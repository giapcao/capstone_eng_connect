# Flow thay doi test cu va test moi (tracked files)

## Pham vi so sanh

- Repo: capstone_eng_connect
- Nhanh: sprint6/feat-course/main
- Pham vi tai lieu nay: test use case trong `EngConnect.Tests`
- Muc tieu thay doi: tang branch coverage bang test data co chu dich, khong dua vao seed ngau nhien/generic case

## Y tuong trung tam cua thay doi

Truoc day, nhieu `*TestData.cs` duoc sinh theo mau chung:

- request duoc tao theo shape co ban
- branch case chu yeu dua vao reflection/source analyzer
- handler case thuong chay bang dependency mac dinh hoac seeded unit of work chung
- nhieu nhanh nghiep vu thuc te chua co mock rieng nen branch coverage con thap

Bay gio, test duoc doi sang huong:

- moi handler duoc doc lai flow nghiep vu
- moi branch quan trong duoc doi thanh case enum rieng trong `*TestData.cs`
- sample data dung gia tri co y nghia nghiep vu, khong random
- dependency duoc mock truc tiep theo tung branch
- valid, boundary, invalid, exception duoc tach ro trong test data

---

## 1) Common test flow

### Test cu

- `UseCaseCaseCatalogFactory` tao du lieu mac dinh theo source analyzer.
- Nhieu case valid/invalid dung chung `InMemoryUnitOfWork` hoac dependency looser.
- Khong phan biet ro branch nghiep vu voi branch ky thuat.

### Test moi

- `Common` giu vai tro test infrastructure dung chung:
  - `UseCaseTestHarness`
  - `UseCaseMockContext`
  - `TestAsyncEnumerable`
  - `TestDependencyFactory`
  - `ObjectGraphAccessor`
- `HandlerTests` va `BranchTests` van giu co che chay chung.
- Phan quyet dinh branch da duoc day xuong tung `*TestData.cs`.

Tac dong:

- Common chi giu framework test.
- Du lieu test va logic branch nam gan use case that.

---

## 2) Flow du lieu test trong moi use case

### Test cu

- Request duoc tao theo 1-2 shape tong quat.
- Case boundary/invalid thuong la bien the co hoc cua validator.
- Khong mock rieng cho cache, transaction, repository, upload service, rollback.

### Test moi

Moi use case duoc doi theo flow:

1. Dinh nghia `enum` case trong `*TestData.cs`.
2. Tao request/sample data tinh cho tung case.
3. Gan `ArrangeMocks` dung dependency can cho branch do.
4. Assert ro:
   - `HttpStatusCode`
   - `Error.Code`
   - `Data`
   - side effect nhu `Update`, `SaveChangesAsync`, `RollbackTransactionAsync`

Tac dong:

- Test khong con phu thuoc vao "seed mac dinh co san co vo tinh dung branch hay khong".
- Moi nhanh can cover phai co case data tuong ung.

---

## 3) Nhom use case da chuyen sang branch-driven data

### 3.1 File/Storage

Da doi `*TestData.cs` de cover branch nghiep vu ro rang:

- `AwsS3Storage/DownloadFile`
  - file ton tai
  - file name boundary
  - file khong ton tai
  - storage throw exception
- `FileStorage/DeleteFileFromDrive`
  - delete thanh cong
  - file id boundary
  - drive tra `false`
  - drive throw exception

### 3.2 Student/Tutor media query

Da doi branch data cho:

- `Students/GetAvatarStudent`
- `Tutors/GetAvatarTutor`
- `Tutors/GetCvUrlTutor`
- `Tutors/GetIntroVideoUrlTutor`

Moi nhom nay da co case:

- cache hit
- repository hit
- entity not found
- media field missing
- cache/service exception

### 3.3 Student/Tutor media update

Da bo sung branch case reject upload cho:

- `Students/UpdateAvatarStudent`
- `Tutors/UpdateAvatarTutor`
- `Tutors/UpdateCvUrlTutor`
- `Tutors/UpdateIntroVideoUrlTutor`

Case moi cover:

- entity ton tai nhung `UpdateFileAsync` tra `null`
- handler tra `Validation.Failed`

### 3.4 Student/User/Tutor profile

Da doi branch data cho:

- `Users/ChangePassword`
  - them case sai `OldPassword`
- `Students/UpdateStatusStudent`
  - them case toggle tu `Inactive -> Active`
- `Students/GetStudentById`
  - them case response khi `Avatar = null`
- `Tutors/GetTutorById`
  - them case response khi `IntroVideoUrl/CvUrl/Avatar = null`
- `Students/CreateStudent`
  - them case duplicate student bang mock ro `userExists = true`, `studentExists = true`

### 3.5 Authentication

Da doi branch data cho:

- `Authentication/RegisterUserStaff`
  - boundary case khi role `Staff` khong ton tai
  - exception case sau khi transaction da bat dau de cover rollback branch

---

## 4) Flow chay test handler moi

### Flow cu

- Tao request
- Cho harness chay handler
- Ky vong pass/fail muc co ban

### Flow moi

Voi moi branch case:

1. `enum` case chon dung request sample.
2. `ArrangeMocks` setup repository/service cho nhanh can test.
3. `BranchTests` goi `UseCaseTestHarness.AssertHandlerCaseAsync`.
4. Harness tao handler that voi override dependency.
5. Test assert ro ket qua va side effect.

Vi du:

- cache hit: mock `GetCacheAsync` + `GetFileUrl`
- duplicate data: mock `AnyAsync = true`
- upload fail: mock `UpdateFileAsync = null`
- transaction rollback: mock `BeginTransactionAsync`, `SaveChangesAsync` throw, `RollbackTransactionAsync`

---

## 5) Validator flow

### Test cu

- Chu yeu dua vao generator tao invalid shape tu source analyzer.

### Test moi

- Van giu co che validator chung cho cac rule co tinh mau.
- Nhung request invalid/boundary quan trong da duoc co dinh trong `*TestData.cs`.

Tac dong:

- Validator khong con phu thuoc vao data random/generic.
- Request shape va boundary duoc giu on dinh qua moi lan regenerate/chay test.

---

## 6) Coverage flow

### Test cu

- Coverage report da co, nhung branch gap kho truy nguon ve tung case cu the.

### Test moi

- Sau moi dot them branch case, test duoc chay lai bang:
  1. `dotnet test`
  2. `scripts/test-coverage.ps1`
  3. `coverlet + ReportGenerator`
- Report van gom trong 1 folder:
  - `artifacts/test-results/coverage`

Tac dong:

- Co the doi chieu nguoc:
  - handler nao chua 100% branch
  - line nao con thieu
  - case nao can bo sung trong `*TestData.cs`

---

## 7) Ket qua hien tai

- Test pass: `1269`
- Coverage hien tai:
  - `EngConnect.Application` line: `93.56%`
  - `EngConnect.Application` branch: `80.55%`
  - `EngConnect.Application` method: `99.4%`

So voi moc truoc khi tiep tuc dot nay:

- Branch coverage tang tu `76.75%` len `80.55%`
- Nhieu handler CRUD/media/status da co branch data ro rang hon

---

## 8) Phan con lai

Cac handler con branch thap chu yeu nam o:

- `UpdateLessonRescheduleRequest`
- `RegisterTutor`
- `LoginWithGoogleOAuth`
- `UpdateTutor`
- `TutorDocuments/*`
- `Meetings/*`
- `LessonScripts/*`
- `GetListStudents`
- `GetListLessons`
- `CreateTutor`
- `UpdateSupportTicketStatus`

Ngoai ra co mot nhom branch do code hien tai tao ra branch kho/khong the cover chi bang test, vi co ternary sau null-guard:

- `GetAvatarStudent`
- `GetAvatarTutor`
- `GetCvUrlTutor`
- `GetIntroVideoUrlTutor`

Vi du flow dang la:

- `if (value != null)` roi ben trong lai co `value != null ? ... : null`
- `if (IsNullOrEmpty(field)) return fail` roi sau do lai co `field != null ? ... : null`

Nhanh `false` cua ternary nay khong co duong di hop le neu khong sua production code.

---

## 9) Tong ket flow cu -> flow moi

Flow test cu:

- generic case
- branch coverage phu thuoc vao seed mac dinh
- kho truy ve branch nghiep vu cu the

Flow test moi:

- case enum ro rang trong tung `*TestData.cs`
- sample data tinh, phu hop valid/boundary/invalid/exception
- mock dependency theo branch
- coverage report co the doi chieu truc tiep voi case da viet

Ket qua:

- test branch-driven de doc, de mo rong, de debug
- de tiep tuc nap them case cho cac handler con lai
- giam rui ro false positive do seed data chung

# Flow thay doi code cu va code moi (tracked files)

## Pham vi so sanh

- Repo: capstone_eng_connect
- Nhanh: sprint6/feat-course/main
- So sanh: tracked files dang modified trong working tree (git diff so voi HEAD)
- Tong quan thay doi: chuyen tu cap nhat truc tiep sang version-chain (clone + relink)
- Pham vi tai lieu nay: CHI mo ta thay doi code (khong bao gom test case/test report)

## Y tuong trung tam cua thay doi

Truoc day, khi sua Module/Session/Resource, he thong cap nhat truc tiep tren ban ghi hien tai.

Bay gio, he thong tao ban ghi moi va giu lien ket ve ban ghi goc qua:

- ParentModuleId
- ParentSessionId
- ParentResourceId

Sau do cap nhat cac bang quan he (many-many) de tro sang ban ghi moi.

Muc tieu flow moi:

- Giu lich su phien ban noi dung
- Co the theo doi nguon goc noi dung khi clone/phan nhanh
- Han che ghi de truc tiep len noi dung dang duoc dung

---

## 1) Domain + Persistence flow

### Code cu

- CourseModule, CourseSession, CourseResource khong co parent chain.
- Khong co self-reference FK cho module/session/resource.

### Code moi

- Them cot parent vao domain model:
  - CourseModule.ParentModuleId
  - CourseSession.ParentSessionId
  - CourseResource.ParentResourceId
- Them navigation self-reference (Parent + InverseParent\*).
- Them EF configuration + FK + index cho 3 parent columns.
- Snapshot migration duoc cap nhat theo mo hinh moi.

Tac dong:

- Moi lan clone tao version moi, co quan he nguoc ve version truoc.

---

## 2) Update flow cua Module

### Code cu

- UpdateCourseModule: sua truc tiep Title/Description/Outcomes tren Module hien tai.
- SaveChanges va tra ve module do.

### Code moi

- Validate module co thuoc course target neu co CourseId trong command.
- Mo transaction.
- Tao Module moi:
  - Id moi
  - ParentModuleId = Id module cu
  - Copy du lieu title/description/outcomes moi
- Chon tap relation can doi (toan bo hoac theo CourseId).
- Relink CourseCourseModule tu module cu sang module moi.
- Commit transaction.
- Tra response theo module moi.

Tac dong:

- Khong ghi de module cu.
- Cung mot module goc co the tao nhieu nhanh theo tung course.

---

## 3) Update flow cua Session

### Code cu

- UpdateCourseSession: sua truc tiep session hien tai.

### Code moi

- Them filter target qua CourseModuleId (optional).
- Validate CourseModuleId phai tham chieu dung relation cua session.
- Mo transaction.
- Tao Session moi:
  - Id moi
  - ParentSessionId = Id session cu
- Relink bang CourseModuleCourseSession sang session moi.
- Commit transaction.
- Tra response session moi.

Tac dong:

- Session duoc version-hoa tuong tu module.

---

## 4) Update flow cua Resource

### Code cu

- UpdateCourseResource sua truc tiep resource.
- Co kiem tra published course trong mot so path.

### Code moi

- Mo transaction.
- Tao Resource moi:
  - Id moi
  - ParentResourceId = Id resource cu
- Relink CourseSessionCourseResource sang resource moi.
- Commit transaction.
- Tra response resource moi.

Tac dong:

- Resource cung theo flow immutable-like (tao moi + doi lien ket).

---

## 5) Add flow: gan noi dung vao Course/Module/Session

### 5.1 Add Module vao Course

Code moi:

- Clone Module
- Khong clone Session
- Tao lai relation CourseModuleCourseSession de Module clone noi den cac Session hien co
- Tao relation CourseCourseModule tro vao Module clone

Khac voi code cu:

- Code cu chi tao relation den module co san.

### 5.2 Add Session vao Module

Code moi:

- Clone Session
- Khong clone Resource
- Tao lai relation CourseSessionCourseResource de Session clone noi den cac Resource hien co
- Tao relation CourseModuleCourseSession tro vao Session clone

Khac voi code cu:

- Code cu chi tao relation den session co san.

### 5.3 Add Resource vao Session

Code moi:

- Clone Resource roi moi tao relation vao Session

Khac voi code cu:

- Code cu gan thang resource co san.

---

## 6) Delete flow thay doi

### Code cu

- Delete module/session/resource theo entity chinh.

### Code moi

- Uu tien xoa relation trung gian truoc:
  - DeleteCourseModule: xoa rows o CourseCourseModule
  - DeleteCourseSession: xoa rows o CourseModuleCourseSession
  - DeleteCourseResource: xoa rows o CourseSessionCourseResource
- Khong con xoa truc tiep entity chinh trong cac handler nay.

Tac dong:

- Hanh vi nghieng ve unlink, giu lai du lieu goc.

---

## 7) Response contract thay doi

Code moi bo sung parent thong tin vao DTO:

- GetCourseModuleResponse: ParentModuleId
- GetCourseSessionResponse: ParentSessionId
- GetCourseResourceResponse: ParentResourceId
- GetCourseResponseDetail (nested): ParentModuleId, ParentSessionId

Mapping va query handlers duoc bo sung map parent fields de API tra day du chain.

---

## 7.1 Get tree theo parent chain (DAG)

Da them 2 ham/query moi de truy vet cay nguoc len parent, chi di theo parent chain:

- getModuleTree: lay module hien tai + danh sach parent den module goc
- getSessionTree: lay session hien tai + danh sach parent den session goc

Nguyen tac DAG ap dung:

- Chi di nguoc parent (ParentModuleId / ParentSessionId)
- Khong truy sang nhanh ngang (khong duyet sibling)
- Dung visited set de tranh loop neu du lieu bi cycle

Output:

- Module/Session hien tai
- ParentChain theo thu tu tu gan den xa (den goc)

---

## 8) Command + Validator thay doi

- UpdateCourseModuleCommand them CourseId? de update co target relation.
- UpdateCourseSessionCommand them CourseModuleId? de update co target relation.
- Validator bo sung rule NotEmpty khi field optional duoc gui.

---

## 9) Transaction flow

Nhieu handler da bo sung flow transaction ro rang:

1. BeginTransactionAsync
2. Add/Update cac ban ghi clone + relation
3. SaveChangesAsync
4. CommitTransactionAsync
5. Neu loi: RollbackTransactionAsync

Tac dong:

- Dam bao tinh toan ven khi clone theo cay va doi relation nhieu bang.

---

## 10) Tong ket flow cu -> flow moi

Flow cu:

- Mutable update (ghi de truc tiep)
- Add relation truc tiep den noi dung co san
- Delete co xu huong xoa entity

Flow moi:

- Versioned content (clone + parent chain)
- Relink relation de su dung ban clone
- Delete theo huong unlink relation
- API response expose parent chain de truy vet lich su

Ket qua nghiep vu:

- De audit va theo doi lich su bien doi noi dung
- Giam nguy co anh huong den noi dung da dung o context khac
- Ho tro mo hinh content reuse + branching tot hon

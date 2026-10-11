# BookManagement - ASP.NET Core .NET 10

Project bài Middleware + Upload hình ảnh cho module Book Management.

## Chạy project
1. Mở `BookManagement.csproj` bằng Visual Studio Community có .NET 10 SDK.
2. Nhấn `F5` hoặc `Ctrl + F5`.
3. Truy cập `/Book`.

## Chức năng Middleware
- Ghi thời gian, Method, Path.
- Ghi Status Code sau `await _next(context)`.
- Chặn `/Book/Detail/0` và `/Book/Detail/-1` với Status Code 400.

## Chức năng Upload hình ảnh
- Form Create cho phép chọn 1 hoặc nhiều ảnh.
- Chấp nhận JPG/JPEG/PNG.
- File khác sẽ báo lỗi.
- Ảnh được lưu tại `wwwroot/uploads/books/`.
- Danh sách sách hiển thị Tên, Giá, Hình ảnh.
- Trang Detail hiển thị các ảnh đã upload.

## Cách test upload
1. Vào `/Book/Create`.
2. Nhập tên sách và giá > 0.
3. Chọn một hoặc nhiều file `.jpg`, `.jpeg`, `.png`.
4. Bấm `Lưu sách`.
5. Quay về `/Book` để thấy ảnh.
6. Thử chọn `.txt` hoặc `.pdf` để minh chứng báo lỗi định dạng.

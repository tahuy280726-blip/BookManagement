# BookManagement - Middleware Assignment (.NET 10)

## Môi trường
- ASP.NET Core MVC
- .NET 10
- Visual Studio Community

## Chức năng đã làm
1. RequestLoggingMiddleware ghi Method + Path trước khi request đi vào Controller.
2. Ghi Status Code sau khi request xử lý xong.
3. Ghi thời gian xử lý request bằng milliseconds.
4. Chặn `/Book/Detail/0` và `/Book/Detail/-1`, trả status 400 và nội dung `Book id không hợp lệ`.
5. BookController có Index, Detail, Create và Validation bằng Data Annotation + ModelState.

## Cách chạy
1. Mở `BookManagement.csproj` bằng Visual Studio Community.
2. Chọn framework .NET 10.
3. Nhấn `Ctrl + F5` hoặc `F5`.
4. Truy cập:
   - `/Book`
   - `/Book/Detail/1`
   - `/Book/Create`
   - `/Book/Detail/0` để kiểm tra status 400.
   - `/Book/Detail/-1` để kiểm tra status 400.
5. Mở cửa sổ **Output** hoặc **Debug Console** để xem log.

## Câu hỏi báo cáo
### 1. Middleware dùng để làm gì?
Middleware là thành phần trung gian trong pipeline HTTP, dùng để xử lý request trước khi đến Controller và có thể xử lý response sau khi Controller xử lý xong.

### 2. Middleware khác Controller ở điểm nào?
Middleware xử lý request/response ở mức pipeline và có thể áp dụng cho nhiều URL. Controller xử lý nghiệp vụ cụ thể của từng route/action và trả về View, dữ liệu hoặc response.

### 3. `await _next(context);` có ý nghĩa gì?
Nó chuyển request sang middleware tiếp theo trong pipeline. Nếu middleware tiếp theo là routing/controller thì request có thể tiếp tục đến Controller.

### 4. Vì sao `return;` khiến request không vào Controller?
Vì middleware kết thúc phương thức `InvokeAsync` ngay tại đó và không gọi `_next(context)`, nên pipeline không được chuyển tiếp.

### 5. Nếu đặt middleware sau `app.MapControllerRoute(...)`?
Có thể middleware không nằm đúng vị trí trong pipeline để xử lý các request đến Controller. Middleware nên được đăng ký trước endpoint mapping để chắc chắn request đi qua middleware trước khi endpoint được thực thi.

### 6. Muốn thêm middleware khác?
Có thể đăng ký tiếp, ví dụ:
`app.UseMiddleware<AnotherMiddleware>();`
Thứ tự đăng ký quyết định thứ tự request đi qua các middleware.

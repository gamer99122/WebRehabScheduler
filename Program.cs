using WebRehabScheduler.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // === 註冊全域 Filter ===
    options.Filters.Add<WebRehabScheduler.Filters.TherapistAuthorizationFilter>();
});


// === 新增 Session 設定 ===
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2); // Session 過期時間：2小時
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// === Session 設定結束 ===

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();

// === 必須在 UseRouting 之後，UseEndpoints 之前 ===
app.UseSession();
// === Session 中介軟體 ===

// === 開發模式：自動設定預設 TherapistId ===
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        if (string.IsNullOrEmpty(context.Session.GetString("TherapistId")))
        {
            context.Session.SetString("TherapistId", "16835");
        }
        await next();
    });
}
// === 開發模式設定結束 ===

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

using Microsoft.EntityFrameworkCore;
using TodoApi;

var builder = WebApplication.CreateBuilder(args);

// הוספת Swagger כדי לייצר תיעוד אוטומטי של ה-API.
builder.Services.AddEndpointsApiExplorer();  // מוסיף את התמיכה בחקר נקודות קצה של ה-API, דרוש עבור Swagger.
builder.Services.AddSwaggerGen();  // מוסיף את הגנרטור של Swagger ליצירת תיעוד אוטומטי של ה-API.

//הוספת שירותי cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()  // מאפשר גישה מכל מקור (domain).
              .AllowAnyMethod()  // מאפשר כל שיטה HTTP (GET, POST, PUT, DELETE וכו').
              .AllowAnyHeader();  // מאפשר כל כותרת HTTP.
    });
});
//חיבור ל-DB
builder.Services.AddDbContext<ToDoDbContext>(options=>
    options.UseMySql(builder.Configuration.GetConnectionString("ToDoDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ToDoDB"))
    )
);

var app = builder.Build();

// הפעלת Swagger, אך רק אם אנחנו בסביבת פיתוח (Development).
// if (app.Environment.IsDevelopment())  
// {
    app.UseSwagger();  // מפעיל את Swagger ומייצר את תיעוד ה-API.
    app.UseSwaggerUI();  // מפעיל את ממשק המשתמש של Swagger כדי לראות את התיעוד ולנסות את ה-API.
// }

app.UseCors();//הפעלת ה cors

app.MapGet("/",()=>"🪬");

app.MapGet("/items",async (ToDoDbContext context) => await context.Items.ToArrayAsync());

app.MapPost("/items",async(ToDoDbContext context,Item item)=>{
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return Results.Created($"/items/{item.Id}", item);
});

app.MapPut("/items/{id}", async (ToDoDbContext context, int id, Item updatedItem) =>
{
    var item = await context.Items.FindAsync(id);  // מחפש את הפריט לפי מזהה ה-ID.
    if (item is null)
        return Results.NotFound();  // אם לא נמצא הפריט, מחזיר תשובת "לא נמצא".
    item.Name = updatedItem.Name;  // מעדכן את שם הפריט.
    item.IsComplete = updatedItem.IsComplete;  // מעדכן אם המשימה הושלמה.
    await context.SaveChangesAsync();  // שומר את השינויים במסד הנתונים.
    return Results.Created($"/items/{item.Id}", item);  // מחזיר תשובה בלי תוכן (200 OK).
});

app.MapDelete("/items/{id}", async (ToDoDbContext context, int id) =>
{
    var item = await context.Items.FindAsync(id);  // מחפש את הפריט לפי מזהה ה-ID.
    if (item is null)
        return Results.NotFound();  // אם לא נמצא הפריט, מחזיר תשובת "לא נמצא".
    context.Items.Remove(item);  // מסיר את הפריט מהטבלה.
    await context.SaveChangesAsync();  // שומר את השינויים במסד הנתונים.
    return Results.NoContent();  // מחזיר תשובה בלי תוכן (200 OK).
});

app.MapGet("/", () => "server API is runing");

app.Run();
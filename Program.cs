using SimpleRestAPI.Data;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
//pelajari lebih lanjut mengenai configuring OpenAPI di https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Dependency Injection
builder.Services.AddSingleton<ICategory, CategoryADO>(); //dengan mengganti ICategoryDal menjadi CategoryADO 
                                                         //maka tidak perlu merubah dari API karena akan dipanggil dari objeknya

builder.Services.AddSingleton<IInstructor, InstructorADO>();

var app = builder.Build(); // Komponen yang digunakan di web REST API

// Configure di HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index => //forecast ini yang pertama kali dijalankan adalah si MapGet nya itu
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("api/v1/helloservices/{name}", (string name) => $"Hello {name}!");

app.MapGet("api/v1/helloservices", (string? id) => 
{
    return $"Hello ASP web API : id={id}";
});

app.MapGet("api/v1/luas-segitiga", (double alas, double tinggi) => 
{
    double luas = 0.5 * alas * tinggi;
    return $"Luas segitiga dengan alas={alas} dan tinggi={tinggi} adalah {luas}";
}); 

app.MapGet("api/v1/categories", (ICategory categoryData) =>
{
    var categories = categoryData.GetCategories();
    return categories;
});

app.MapGet("api/v1/categories/{id}", (ICategory categoryData, int id) =>
{
    var category = categoryData.GetCategoryById(id);
    return category;
});

app.MapPost("api/v1/categories", (ICategory categoryData,Category category) =>
{
    var newCategory = categoryData.AddCategory(category);
    return newCategory;
});

app.MapPut("api/v1/categories", (ICategory categoryData,Category category) =>
{
    var updateCategory = categoryData.UpdateCategory(category);
    return updateCategory;
});

app.MapDelete("api/v1/categories/{id}", (ICategory categoryData, int id) =>
{
    categoryData.DeleteCategory(id);
    return Results.NoContent(); 
});


app.MapGet("api/v1/instructors", (IInstructor instructorData) =>
{
    var instructors = instructorData.GetInstructors();
    return instructors;
});

app.MapGet("api/v1/instructors/{id}", (IInstructor instructorData, int id) =>
{
    var instructor = instructorData.GetInstructorById(id);
    return instructor;
});

app.MapPost("api/v1/instructors", (IInstructor instructorData, Instructor instructor) =>
{
    var newInstructor = instructorData.AddInstructor(instructor);
    return newInstructor;
});

app.MapPut("api/v1/instructors", (IInstructor instructorData, Instructor instructor) =>
{
    var updateInstructor = instructorData.UpdateInstructor(instructor);
    return updateInstructor;
});

app.MapDelete("api/v1/instructors/{id}", (IInstructor instructorData, int id) =>
{
    instructorData.DeleteInstructor(id);
    return Results.NoContent();
}); 


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}



// Tugas: Cari tahu soal RESTful API
// - Apa bedanya sama protokol lain?
// - Kenapa kita pakai RESTful API?

// RESTful API itu cara buat aplikasi ngobrol lewat internet pake HTTP
// Simpel, fleksibel, dan standar banget buat web & mobile apps

// SOA vs REST API?  
// SOA tuh konsep lama, pake banyak protokol. REST API lebih modern, simpel, pake HTTP

// Cara bikin API sendiri?  
// Tinggal tambahin endpoint pake `app.MapGet`, `app.MapPost`, `app.MapPut`, `app.MapDelete`
// Contohnya:  
// app.MapGet("api/v1/helloservices", () => "Hello from services!");

// - MapGet  → Buat nampilin data  
// - MapPost → Buat nambah data  
// - MapPut  → Buat update data  
// - MapDelete → Buat hapus data  
 
//percobaan untuk ngetes pakai insomnia
// Data masih pake List, nanti bisa pakai database agar bisa kesimpan 8i

// Membuat instruktor seperti yang bapanya buatkan di kelas, kemudian dibuatkan berdasarkan yang telah bapanya buat dikelas
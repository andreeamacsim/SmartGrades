using System.Reflection;
using System.Text;
using BackEnd.Service;
using BackEnd.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials()
                                  .WithExposedHeaders("Authorization");

                              });
});

BsonSerializer.TryRegisterSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));


builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddSingleton<ICourseCollectionService, CourseCollectionService>();
builder.Services.AddSingleton<IGradeCollectionService, GradeCollectionService>();
builder.Services.AddSingleton<ITeacherCollectionService, TeacherCollectionService>();
builder.Services.AddSingleton<IStudentCollectionService, StudentCollectionService>();
builder.Services.AddScoped<IEmailService, EmailService>();



builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JxSpW.NmvsHpgstntUYSMP2065.Fiutnbriu..6895")),
        ValidateAudience = false,
        ValidateIssuer = false,
    };
});

var app = builder.Build();
app.UseCors("CorsPolicy");
app.UseRouting();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.Run();

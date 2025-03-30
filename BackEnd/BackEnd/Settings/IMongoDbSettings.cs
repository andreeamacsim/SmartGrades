namespace BackEnd.Settings
{
    public interface IMongoDbSettings
    {
        string StudentsCollectionName { get; set; }
        string TeachersCollectionName { get; set; }
        string GradesCollectionName { get; set; }
        string CoursesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

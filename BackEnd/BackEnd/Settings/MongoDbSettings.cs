namespace BackEnd.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string StudentsCollectionName { get; set; }
        public string TeachersCollectionName { get; set; }
        public string GradesCollectionName { get ; set ; }
        public string CoursesCollectionName { get ; set ; }
        public string ConnectionString { get ; set; }
        public string DatabaseName { get; set; }
    }
}

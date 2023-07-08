~~~bash
dotnet aspnet-codegenerator razorpage     -m Course     -dc ApplicationDbContext     -udl     -outDir Pages/Courses     --referenceScriptLibraries    -f
dotnet aspnet-codegenerator razorpage     -m Homework     -dc ApplicationDbContext     -udl     -outDir Pages/Homeworks     --referenceScriptLibraries    -f
dotnet aspnet-codegenerator razorpage     -m Person     -dc ApplicationDbContext     -udl     -outDir Pages/Persons     --referenceScriptLibraries    -f
dotnet aspnet-codegenerator razorpage     -m PersonCourse     -dc ApplicationDbContext     -udl     -outDir Pages/PersonCourses     --referenceScriptLibraries    -f
dotnet aspnet-codegenerator razorpage     -m PersonCourseHomework     -dc ApplicationDbContext     -udl     -outDir Pages/PersonCourseHomeworks     --referenceScriptLibraries    -f


dotnet aspnet-codegenerator razorpage     -m CheckersGame     -dc AppDbContext     -udl     -outDir Pages/CheckersGames     --referenceScriptLibraries    -f
dotnet aspnet-codegenerator razorpage     -m CheckersOption     -dc AppDbContext     -udl     -outDir Pages/CheckersOptions     --referenceScriptLibraries    -f
dotnet aspnet-codegenerator razorpage     -m CheckersGameState     -dc AppDbContext     -udl     -outDir Pages/CheckersGameStates     --referenceScriptLibraries    -f
~~~

dotnet ef migrations add InitialCreate --project DAL.Db --startup-project ConsoleApp
dotnet ef database update --project DAL.Db --startup-project ConsoleApp
dotnet ef database drop --project DAL.Db --startup-project ConsoleApp 





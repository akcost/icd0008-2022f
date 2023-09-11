# CheckersGame

Simple Checkers game that you can play through internet browser or the console(cmd).

Play against another player or computer.

## Database setup

1. Go to the `WebApp` project
2. Open `appsettings.json`
3. Edit DefaultConnection with your own path to where you want .db file to be stored at
4. Do the same for `AppDbContextFactory.cs` in `DAL.Db`project and `Program.cs` in `ConsoleApp` project.
5. Run these commands below

#### Create migration
dotnet ef migrations add InitialCreate --project DAL.Db --startup-project ConsoleApp
#### Apply migration
dotnet ef database update --project DAL.Db --startup-project ConsoleApp

You can now run the Checkers game from the ConsoleApp, Webapp or both at the same time. Player 1 can play from WebApp while Player 2 plays from Console. 


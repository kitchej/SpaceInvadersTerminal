
public class Logger{
    string _logFile;

    public Logger(string logFile){
        _logFile = logFile;
    }

    public async void Log(string message){
        using StreamWriter file = new(_logFile, append: true);
        await file.WriteLineAsync(message);
    }
}
class ServiceRecord
{
    public string Name { get; set; } = "";
    public bool IsRunning { get; set; }
    public int RestartCount { get; set; }
    public void Start()
    {
        IsRunning = true;
    }
    public void Stop()
    {
        IsRunning = false;
    }
    public void Restart()
    {
        if (IsRunning)
        {
            IsRunning = true;
            RestartCount++; 
        } else
        {
            Start();
        }
    }
}

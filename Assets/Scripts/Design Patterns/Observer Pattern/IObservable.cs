public interface IObservable
{
    //subject uses this method to notify all observers.
    public void OnNotify(PowerupPanelActions action);
}
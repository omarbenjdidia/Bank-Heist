namespace eWolf.eWolfDynamicVDU.Scripts.Interfaces
{
    public interface IReciveEffectMessages
    {
        void UpdateAlert(float alert);

        void SetActive(bool paused);
    }
}
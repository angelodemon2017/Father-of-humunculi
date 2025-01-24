internal class QuestData
{
    internal string Key;

    private QuestConfig _cashConfig;

    internal bool IsDone => GetConfig.IsDone(UIPlayerManager.Instance.EntityMonobeh);

    internal QuestConfig GetConfig
    {
        get
        {
            if (_cashConfig == null)
            {
                _cashConfig = QuestLibrary.Instance.GetQuestConfigByKey(Key);
            }

            return _cashConfig;
        }
    }

    internal string Description 
    {
        get 
        {
            string result = GetConfig.GetState(UIPlayerManager.Instance.EntityMonobeh);

            return result;
        }
    }
}
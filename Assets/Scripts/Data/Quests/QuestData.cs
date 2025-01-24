internal class QuestData
{
    internal string Key;

    private QuestConfig _cashConfig;

    private QuestConfig GetConfig
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

    private void CheckRule()
    {
        if (GetConfig.IsDone(UIPlayerManager.Instance.EntityMonobeh))
        {
            //TODO
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
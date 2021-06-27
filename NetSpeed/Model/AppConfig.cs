namespace NetSpeed.Model
{
    internal class AppConfig
    {
        /// <summary>
        /// 已选择的适配器的ID (GUID)
        /// </summary>
        public string AdapterId { get; set; }

        /// <summary>
        /// 刷新间隔（毫秒）
        /// </summary>
        public int RefreshInterval { get; set; }
    }
}

namespace UMLEditort
{
    /// <summary>
    /// 目前的使用者操作的模式
    /// </summary>
    public enum Modes
    {
        Undefined,
        Select,
        Associate,
        Generalize,
        Composition,
        Class,
        UseCase
    }

    /// <summary>
    /// 基本物件的四個 Connection Ports
    /// </summary>
    public enum Ports
    {
        Undefined,
        Top,
        Right,
        Bottom,
        Left
    }

}
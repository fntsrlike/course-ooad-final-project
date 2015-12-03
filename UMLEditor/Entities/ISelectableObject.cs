using System.Windows;

namespace UMLEditort.Entities
{
    public interface ISelectableObject
    {
        /// <summary>
        /// 是否已被選取
        /// </summary>
        bool Selected
        {
            get;
            set;
        }

        /// <summary>
        /// 本物件所屬的組合物件
        /// </summary>
        CompositeObject Compositer
        {
            get;
            set;
        }

        /// <summary>
        /// 本物件所屬組合中的最外層的組合物件
        /// </summary>
        /// <returns></returns>
        CompositeObject GetOutermostCompositer();
    }
}

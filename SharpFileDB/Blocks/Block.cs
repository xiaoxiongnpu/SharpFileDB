﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
//using System.Threading.Tasks;

namespace SharpFileDB.Blocks
{
    /// <summary>
    /// 存储到数据库文件的一块内容。
    /// </summary>
    [Serializable]
    public abstract class Block : ISerializable
    {

#if DEBUG

        /// <summary>
        /// 创建新<see cref="Block"/>时应设置其<see cref="Block.blockID"/>为计数器，并增长此计数器值。
        /// </summary>
        internal static long IDCounter = 0;

        /// <summary>
        /// 用于给此块标记一个编号，仅为便于调试之用。
        /// </summary>
        public long blockID;
#endif

        /// <summary>
        /// 此对象自身在数据库文件中的位置。为0时说明尚未指定位置。只有<see cref="DBHeaderBlock"/>的位置才应该为0。
        /// <para>请注意在读写时设定此值。</para>
        /// </summary>
        public long ThisPos { get; set; }

        /// <summary>
        /// 存储到数据库文件的一块内容。
        /// </summary>
        public Block()
        {
#if DEBUG
            this.blockID = IDCounter++;
#endif
            BlockCache.AddFloatingBlock(this);
        }

#if DEBUG
        const string strBlockID = "";
#endif

        #region ISerializable 成员

        /// <summary>
        /// 序列化时系统会调用此方法。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
#if DEBUG
            info.AddValue(strBlockID, this.blockID);
#endif
        }

        #endregion

        /// <summary>
        /// BinaryFormatter会通过调用此方法来反序列化此块。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected Block(SerializationInfo info, StreamingContext context)
        {
#if DEBUG
            this.blockID = info.GetInt64(strBlockID);
#endif
        }

        /// <summary>
        /// 显示此块的信息，便于调试。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
#if DEBUG
            return string.Format("{0}: ID:{1}, Pos: {2}", this.GetType().Name, this.blockID, this.ThisPos);
#else
            return string.Format("{0}: Pos: {1}", this.GetType().Name, this.ThisPos);
#endif
        }

        /// <summary>
        /// 安排所有文件指针。如果全部安排完毕，返回true，否则返回false。
        /// </summary>
        /// <returns></returns>
        public abstract bool ArrangePos();
    }
}

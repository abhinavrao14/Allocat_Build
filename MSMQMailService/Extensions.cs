using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;

namespace MSMQMailService
{
   public static class Extensions
    {
        /// <summary>
        /// Extension method to count messages in a message queue
        /// </summary>
        /// <param name="q">The extended object</param>
        /// <returns></returns>
        public static int GetMessageCount(this MessageQueue q)
        {

            int messageCount = 0;
            using (MessageEnumerator messageEnumerator = q.GetMessageEnumerator2())
            {
                while (messageEnumerator.MoveNext())
                {
                    messageCount++;
                }
            }
            return messageCount;
        }

        /// <summary>
        /// Extension method to get the id of the next elegible message. 
        /// That is the one with the oldest allready past scheduled time (stored in the extension property)
        /// </summary>
        /// <param name="q">The extended object</param>
        /// <returns></returns>

        public static String GetScheduledMessageID(this MessageQueue q)
        {
            DateTime OldestTimestamp = DateTime.MaxValue;
            String OldestMessageID = null;

            using (MessageEnumerator messageEnumerator = q.GetMessageEnumerator2())
            {
                while (messageEnumerator.MoveNext())
                {
                    DateTime ScheduledTime = DateTime.FromBinary(BitConverter.ToInt64(messageEnumerator.Current.Extension, 0));
                    if (ScheduledTime < DateTime.UtcNow) // Take only the proper ones 
                    {
                        if (ScheduledTime < OldestTimestamp)
                        {
                            OldestTimestamp = ScheduledTime;
                            OldestMessageID = messageEnumerator.Current.Id;
                        }
                    }
                }
            }
            return OldestMessageID;
        }
    }
}

 using System.Runtime.Serialization;
 using UnityEngine;
 
 sealed class QuaternionSerializationSurrogate : ISerializationSurrogate
{
     // Method called to serialize a Vector3 object
     public void GetObjectData(object obj,
                               SerializationInfo info, StreamingContext context) {
         
         Quaternion q = (Quaternion) obj;
         info.AddValue("x", q.x);
         info.AddValue("y", q.y);
         info.AddValue("z", q.z);
         info.AddValue("w", q.w);
         Debug.Log(q);
     }
     
     // Method called to deserialize a Vector3 object
     public object SetObjectData(object obj,
                                        SerializationInfo info, StreamingContext context,
                                        ISurrogateSelector selector) {
         
         Quaternion q = (Quaternion) obj;
         q.x = (float)info.GetValue("x", typeof(float));
         q.y = (float)info.GetValue("y", typeof(float));
         q.z = (float)info.GetValue("z", typeof(float));
         q.w = (float)info.GetValue("w", typeof(float));
         obj = q;
         return obj;   // Formatters ignore this return value //Seems to have been fixed!
     }
 }

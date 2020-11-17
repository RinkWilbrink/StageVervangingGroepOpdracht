using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Source of the code below
 * 
 * https://github.com/Zeejfps/Unity-Achievement-System
 * 
*/

namespace Achievements
{
    [CreateAssetMenu()]
    public class AchievementDatabase : ScriptableObject
    {
        // A list of all achievements
        public List<Achievement> achievementsList;
    }
}
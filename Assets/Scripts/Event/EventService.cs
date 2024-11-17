

using System;

/**  This script demonstrates implementation of the Observer Pattern.
*  If you're interested in learning about Observer Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/
namespace StatePattern.Events
{
    public class EventService
    {
        public EventController<int> OnLevelSelected { get; private set; }
        public EventController<int> OnPointGained { get; private set; }

        public EventService()
        {
            OnLevelSelected = new EventController<int>();
            OnPointGained = new EventController<int>();
        }
    }
}
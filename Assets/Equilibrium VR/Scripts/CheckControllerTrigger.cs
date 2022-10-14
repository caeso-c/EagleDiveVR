using UnityEngine;
using UnityEngine.XR;

    public class CheckControllerTrigger : MonoBehaviour
    {
    public GameLogic manager;

        public int[] thrustChanges = new int[3];
        int currentThrust = -1;
        int count = 0;

        public enum EventQuickSelect
        {
            Custom,
            None,
            All,
            ButtonOnly,
            AxisOnly,
            SenseAxisOnly
        }

        // public VRTK_ControllerEvents controllerEvents;

        // [Header("Quick Select")]

        // public EventQuickSelect quickSelect = EventQuickSelect.All;

        // [Header("Button Events Debug")]

        // public bool triggerButtonEvents = true;
        

        private void OnEnable()
        {
            // controllerEvents = (controllerEvents == null ? GetComponent<VRTK_ControllerEvents>() : controllerEvents);
            // if (controllerEvents == null)
            // {
            //     VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            //     return;
            // }

            //Setup controller event listeners

            //controllerEvents.TriggerClicked += DoTriggerClicked;

            //change to default thrust
            //currentThrust = thrustChanges[0];
        }

        private void OnDisable()
        {
            //if (controllerEvents != null)
            //{
            //    controllerEvents.TriggerClicked -= DoTriggerClicked; 
            //}
        }

        private void LateUpdate()
        {
            // switch (quickSelect)
            // {
            //     case EventQuickSelect.None:
            //         triggerButtonEvents = false;
            //         break;
            //     case EventQuickSelect.All:
            //         triggerButtonEvents = true;
            //         break;
            //     case EventQuickSelect.ButtonOnly:
            //         triggerButtonEvents = true;
            //         break;

            // }
        }

        // private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
        // {
        //     string debugString = "Controller on index '" + index + "' " + button + " has been " + action
        //                          + " with a pressure of " + e.buttonPressure + " / Primary Touchpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)" ;
            
        // }



        // private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
        // {
        //     if (triggerButtonEvents)
        //     {
        //         //-DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "clicked", e);

        //         currentThrust = thrustChanges[count % 3];
        //         manager.thrust = currentThrust;    
        //         count++; 
        //     }
        // }

        // private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
        // {
        //     // if (triggerButtonEvents)
        //     // {
        //     //     DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "unclicked", e);
        //     // }
        // }

       
    }


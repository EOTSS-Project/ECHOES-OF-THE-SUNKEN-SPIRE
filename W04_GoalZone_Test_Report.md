# Week 04 - GoalZone Animation and Delivery Trigger Test Report

**Project:** Echoes of the Sunken Spire  
**Task:** Person 6 - GoalZone Animation and Delivery Trigger Verification  
**Implemented by:** Tuğçe Naz Eser

## 1. GoalZone Animation Tests
- Verified that GoalZoneEffects.cs is attached to ExitDoor_Zone.
- Confirmed that the GoalZone rotates correctly using Space.Self.
- Verified that the Mathf.Sin-based pulse animation works in Play Mode.
- Checked the configured animation parameters:
  - Rotation Speed: 25
  - Pulse Speed: 2.5
  - Pulse Amount: 0.15

**Result:** Passed.

## 2. Crystal Collection and Delivery Tests
- Verified player movement using WASD controls.
- Confirmed successful crystal collection through the "Collected!" Console message.
- Observed the "Delivered!" Console message, confirming that the delivery event was triggered.
- Confirmed that the hazard interaction can trigger cargo dropping.

**Result:** Delivery event triggered successfully.

## 3. Level Accessibility Observation
- The player could not reach the elevated GoalZone through normal movement during the test.
- A temporary Player position adjustment was used in Play Mode to test the delivery interaction.
- The normal gameplay route to the GoalZone still requires verification.

## 4. Final Notes
- GoalZone rotation and pulse effects worked during testing.
- The delivery trigger produced the expected Console message.
- No permanent changes were made to existing scripts or scene objects.
- Further verification of the normal route to the GoalZone is recommended.
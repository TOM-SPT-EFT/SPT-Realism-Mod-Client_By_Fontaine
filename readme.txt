Dayz has a blood system that counts for total blood volume and this can lead to repeated in-and-out of consciousness and even fataility.

Human Blood Volume (Male):
The average adult male has approximately 5 to 6 liters (5000 - 6000 mL) of blood in their body.
Blood volume is typically estimated as about 7-8% of total body weight. For a 70-80 kg male, this translates to around 5-6 liters.
Blood Loss Thresholds and Effects:
Blood loss is categorized into four classes, and each class has distinct physiological effects. These effects can guide your gameplay system for loss of consciousness, shock, etc.

1. Class I: Mild Blood Loss (0-15% of total blood volume)
Blood loss: Up to 750 mL (assuming 5 liters total blood).
Symptoms:
The subject is generally asymptomatic with minimal to no effects.
The body compensates, so heart rate and blood pressure remain stable.
No loss of consciousness or cognitive impairment.
Game mechanics: Little to no effect on gameplay.
2. Class II: Moderate Blood Loss (15-30% of total blood volume)
Blood loss: 750 - 1500 mL.
Symptoms:
Increased heart rate (tachycardia), up to 100-120 bpm.
Slightly lowered blood pressure.
Skin becomes cool, clammy, and pale due to peripheral vasoconstriction.
Mild cognitive impairment, difficulty concentrating.
Game mechanics: Minor impact on movement speed or stamina, slight visual effects (blur, color fade), slight cognitive impairment.
3. Class III: Severe Blood Loss (30-40% of total blood volume)
Blood loss: 1500 - 2000 mL.
Symptoms:
Significant tachycardia (120-140 bpm).
Decreased blood pressure, dizziness.
Confusion, anxiety, and loss of consciousness may start to occur.
Hypovolemic shock: The body’s organs receive insufficient blood.
Game mechanics: Reduced mobility, severe blurring of vision, balance issues, potential loss of consciousness. Player may need immediate medical intervention or risk death.
4. Class IV: Critical Blood Loss (>40% of total blood volume)
Blood loss: Over 2000 mL.
Symptoms:
Severe shock, extremely fast heart rate (over 140 bpm).
Confusion, loss of consciousness, and immediate risk of death.
Organ failure, death imminent without medical intervention.
Game mechanics: Immediate loss of consciousness or death, screen blackout, possibly some final visual effects (severe color desaturation).
Variables for Loss over Time (Bleeding Rate):
To simulate blood loss over time and its effects, you’ll need to model:

Bleeding rate: How fast blood is lost, typically measured in mL/min. This varies based on the severity of the wound:

Small wounds: ~10-30 mL/min.
Larger, more severe wounds: 100-200+ mL/min.
Major arterial wounds: Can exceed 500 mL/min or more.
Duration of blood loss: The total blood loss will be the rate multiplied by the duration.

Example: Losing 200 mL/min for 10 minutes results in 2000 mL of blood loss (40% of total blood volume).
Cumulative effects: If blood loss happens over several minutes:

Gradual effects can simulate shock, cognitive impairment, and reduced stamina.
Sudden, rapid blood loss will have a much more dramatic effect, possibly leading to quick unconsciousness.
Shock and Loss of Consciousness:

Shock: Occurs when the body’s organs and tissues don’t get enough oxygen due to reduced blood flow. This can result from loss of 20-30% of blood volume and typically begins to appear after Class III blood loss.

Loss of Consciousness: Usually occurs with Class III or IV blood loss (30-40% or more). The body prioritizes vital organs, reducing blood flow to the brain, causing fainting or unconsciousness.

Proposed Variables for this Mod:
Total Blood Volume (TBV): 5000 mL for a standard male.
Bleeding rate (mL/min): Depending on the wound, can range from 10 mL/min to 500+ mL/min for severe wounds.
Blood loss thresholds:
Mild (0-15%): No gameplay impact.
Moderate (15-30%): Tachycardia, mild visual impairment, stamina reduction.
Severe (30-40%): Shock, loss of coordination, possible loss of consciousness.
Critical (>40%): Immediate unconsciousness or death.
Example Calculation:
Assume a player has a severe wound losing 200 mL/min. Over 10 minutes, they lose 2000 mL, which is 40% of their total blood volume.
After 5 minutes (1000 mL loss, 20% of total), they may start experiencing shock and cognitive impairment.
After 10 minutes, they will likely lose consciousness unless the bleeding is stopped.
Next Steps in this mod addition:
Track Blood Volume: Implement a variable to store the player’s total blood volume and dynamically adjust it based on bleeding events.
Track Bleeding Rate: Different wounds should have different bleeding rates.
Apply Blood Loss Effects: Add gameplay mechanics that correspond to blood loss percentages (e.g., vision blur, decreased stamina, loss of consciousness).
Stop Bleeding: Allow players to use items (like bandages) to stop or reduce bleeding rates.

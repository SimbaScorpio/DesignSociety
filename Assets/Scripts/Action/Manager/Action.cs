﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignSociety
{
	public interface IActionCompleted
	{
		void OnActionCompleted (Action action);
	}


	public class Action : MonoBehaviour
	{
		public void Free ()
		{
			Destroy (this);	// 卸载脚本
		}
	}

	public class ActionSingle : Action
	{
		// 独立动作
	}

	public class ActionThread : Action
	{
		// 同存动作
	}


	public class ActionName
	{
		static Dictionary<string, ActionType> nameIsValid = new Dictionary<string, ActionType> ();
		static Dictionary<string, string> nameWithBorder = new Dictionary<string, string> ();
		static Dictionary<string, string[]> nameWithItem = new Dictionary<string, string[]> ();

		// 判断动作是否合法，返回动作类型
		public static ActionType IsValid (string actionName)
		{
			if (!string.IsNullOrEmpty (actionName) && nameIsValid.ContainsKey (actionName))
				return nameIsValid [actionName];

			string[] sitActionNames = System.Enum.GetNames (typeof(SitActionName));
			for (int i = 0; i < sitActionNames.Length; ++i) {
				if (actionName == sitActionNames [i]) {
					nameIsValid [actionName] = ActionType.sit;
					return ActionType.sit;
				}
			}
			string[] standActionNames = System.Enum.GetNames (typeof(StandActionName));
			for (int i = 0; i < standActionNames.Length; ++i) {
				if (actionName == standActionNames [i]) {
					nameIsValid [actionName] = ActionType.stand;
					return ActionType.stand;
				}
			}
			nameIsValid [actionName] = ActionType.error;
			return ActionType.error;
		}


		// 返回含有起始和结束动作的根动作名称（不包含begin/end），如果没有则返回null
		public static string FindBorder (string actionName)
		{
			if (!string.IsNullOrEmpty (actionName) && nameWithBorder.ContainsKey (actionName))
				return nameWithBorder [actionName];

			string border;
			string[] sitActionNames = System.Enum.GetNames (typeof(SitActionWithBorder));
			for (int i = 0; i < sitActionNames.Length; ++i) {
				if (actionName == sitActionNames [i]) {
					border = GetSitRoot (actionName);
					nameWithBorder [actionName] = border;
					return border;
				}
			}
			string[] standActionNames = System.Enum.GetNames (typeof(StandActionWithBorder));
			for (int i = 0; i < standActionNames.Length; ++i) {
				if (actionName == standActionNames [i]) {
					border = GetStandRoot (actionName);
					nameWithBorder [actionName] = border;
					return border;
				}
			}
			return null;
		}


		// 特殊的，处理具有相同根动作的动作，需确保输入的动作是有起始和结束动作的
		public static string GetSitRoot (string actionName)
		{
			if (actionName == SitActionWithBorder.sit_cross_arm_shake_head.ToString ())
				return SitActionWithBorder.sit_cross_arm.ToString ();

			if (actionName == SitActionWithBorder.sit_keyboard.ToString ())
				return SitActionWithBorder.sit_computer.ToString ();
			if (actionName == SitActionWithBorder.sit_mouse_move.ToString ())
				return SitActionWithBorder.sit_computer.ToString ();
			if (actionName == SitActionWithBorder.sit_mouse_click.ToString ())
				return SitActionWithBorder.sit_computer.ToString ();
			if (actionName == SitActionWithBorder.sit_scratch_head_computer.ToString ())
				return SitActionWithBorder.sit_computer.ToString ();
			if (actionName == SitActionWithBorder.sit_chin_in_hand_computer.ToString ())
				return SitActionWithBorder.sit_computer.ToString ();

			if (actionName == SitActionWithBorder.sit_fixphone_dial.ToString ())
				return SitActionWithBorder.sit_fixphone.ToString ();

			if (actionName == SitActionWithBorder.sit_play_cellphone_click.ToString ())
				return SitActionWithBorder.sit_hold_cellphone.ToString ();
			if (actionName == SitActionWithBorder.sit_play_cellphone_scroll.ToString ())
				return SitActionWithBorder.sit_hold_cellphone.ToString ();

			if (actionName == SitActionWithBorder.sit_listen_cellphone.ToString ())
				return SitActionWithBorder.sit_talk_cellphone.ToString ();

			if (actionName == SitActionWithBorder.sit_ipad_click.ToString ())
				return SitActionWithBorder.sit_ipad.ToString ();
			if (actionName == SitActionWithBorder.sit_ipad_scroll.ToString ())
				return SitActionWithBorder.sit_ipad.ToString ();

			if (actionName == SitActionWithBorder.sit_vr_handle.ToString ())
				return SitActionWithBorder.sit_vr.ToString ();

			if (actionName == SitActionWithBorder.sit_wine_toast.ToString ())
				return SitActionWithBorder.sit_wine.ToString ();

			if (actionName == SitActionWithBorder.sit_eat_with_tableware.ToString ())
				return SitActionWithBorder.sit_with_tableware.ToString ();
			if (actionName == SitActionWithBorder.sit_talk_with_tableware.ToString ())
				return SitActionWithBorder.sit_with_tableware.ToString ();
			return actionName;
		}


		// 特殊的，处理具有相同根动作的动作，需确保输入的动作是有起始和结束动作的
		public static string GetStandRoot (string actionName)
		{
			if (actionName == StandActionWithBorder.stand_cross_arm_shake_head.ToString ())
				return StandActionWithBorder.stand_cross_arm.ToString ();
			
			if (actionName == StandActionWithBorder.stand_fixphone_dial.ToString ())
				return StandActionWithBorder.stand_fixphone.ToString ();
			
			if (actionName == StandActionWithBorder.stand_play_cellphone_click.ToString ())
				return StandActionWithBorder.stand_hold_cellphone.ToString ();
			if (actionName == StandActionWithBorder.stand_play_cellphone_scroll.ToString ())
				return StandActionWithBorder.stand_hold_cellphone.ToString ();
			
			if (actionName == StandActionWithBorder.stand_listen_cellphone.ToString ())
				return StandActionWithBorder.stand_talk_cellphone.ToString ();
			
			if (actionName == StandActionWithBorder.stand_ipad_click.ToString ())
				return StandActionWithBorder.stand_ipad.ToString ();
			if (actionName == StandActionWithBorder.stand_ipad_scroll.ToString ())
				return StandActionWithBorder.stand_ipad.ToString ();
			
			if (actionName == StandActionWithBorder.stand_wine_toast.ToString ())
				return StandActionWithBorder.stand_wine.ToString ();
			
			if (actionName == StandActionWithBorder.other_exercise_3.ToString ())
				return StandActionWithBorder.other_exercise_2.ToString ();

			if (actionName == StandActionWithBorder.stand_vr_handle.ToString ())
				return StandActionWithBorder.stand_vr.ToString ();

			if (actionName == StandActionWithBorder.stand_keyboard.ToString ())
				return StandActionWithBorder.stand_computer.ToString ();
			if (actionName == StandActionWithBorder.stand_mouse_move.ToString ())
				return StandActionWithBorder.stand_computer.ToString ();
			if (actionName == StandActionWithBorder.stand_mouse_click.ToString ())
				return StandActionWithBorder.stand_computer.ToString ();
			if (actionName == StandActionWithBorder.stand_scratch_head_computer.ToString ())
				return StandActionWithBorder.stand_computer.ToString ();
			
			return actionName;
		}

		// 返回动作附带的物品路径
		public static string[] FindItems (string stateName)
		{
			if (!string.IsNullOrEmpty (stateName) && nameWithItem.ContainsKey (stateName))
				return nameWithItem [stateName];
			string[] items = FindItemsSearch (stateName);
			nameWithItem [stateName] = items;
			return items;
		}

		public static string[] FindItemsSearch (string stateName)
		{
			switch (stateName) {
			// A - normal display (object under such path simply set shown or hidden when animation begin or end)
			case "stand_talk_mic":
				return new string[] {
					"A_mic_1"
				};
			case "stand_talk_paper":
				return new string[] {
					"A_paper_1"
				};
			case "stand_talk_megaphone":
				return new string[] {
					"A_megaphone_1"
				};
			case "stand_talk_megaphone_point_far_times":
				return new string[] {
					"A_megaphone_1"
				};
			case "stand_talk_interphone":
				return new string[] {
					"A_interphone_1"
				};
			case "stand_talk_interphone_point_far_times":
				return new string[] {
					"A_interphone_1"
				};
			case "stand_hold_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "stand_play_cellphone_click":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "stand_play_cellphone_scroll":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "stand_talk_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/A_cellphone_2"
				};
			case "stand_listen_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/A_cellphone_2"
				};
			case "stand_ipad":
				return new string[] {
					"A_ipad_1"
				};
			case "stand_ipad_click":
				return new string[] {
					"A_ipad_1"
				};
			case "stand_ipad_scroll":
				return new string[] {
					"A_ipad_1"
				};
			case "stand_write_pen":
				return new string[] {
					"A_pen_1",
					"A_paper_2"
				};
			case "stand_camera":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/A_camera_1"
				};
			case "stand_tailor_tape_model":
				return new string[] {
					"A_tailor_tape_1"
				};
			case "stand_write_wall":
				return new string[] {
					"A_pen_2"
				};
			case "stand_controller":
				return new string[] {
					"A_controller_1"
				};
			case "stand_drink_water":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_watercup_1"
				};
			case "stand_insert":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_insertcube_1",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_insertstick_1"
				};
			case "stand_light":
				return new string[] {
					"A_spotlight_1"
				};
			case "stand_mic_self":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_thumb/A_mic_2"
				};
			case "stand_mic_other":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_thumb/A_mic_2"
				};
			case "stand_photo_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/A_cellphone_2"
				};
			case "stand_pour_wine":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_thumb/left_thumbmid/A_winebottle_1"
				};
			case "stand_record_rod":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/right_arm/right_elbow/right_hand/right_mid/right_mid2/A_mic_3"
				};
			case "stand_recorder":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/left_mid2/A_pen_3"
				};
			case "stand_tailor_tape":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_tailor_tape_2"
				};
			case "stand_water_flower":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/left_mid2/A_waterpot_1"
				};
			case "stand_remote":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/left_mid2/A_remote_control_1"
				};
			case "stand_cut":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_scissors_1"
				};
			case "stand_grinder":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_grinder_1"
				};
			case "stand_iron":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_iron_1"
				};
			case "stand_saws":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_saws_1"
				};
			case "stand_solder":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_solder_L_1",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_solder_R_1"
				};
			case "stand_steel_tape":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_steel_tape_1"
				};
			case "stand_scratch_cd":
				return new string[] {
					"A_headset_1"
				};
			case "stand_serve_dish":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/A_coffe_cup_1",
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/A_coffe_cup_2",
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/A_plate_1"
				};
			case "stand_take_dish":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/A_coffe_cup_1",
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/A_coffe_cup_2",
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_mid/A_plate_1"
				};
			case "stand_pipe":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pipe_1"
				};
			case "stand_bartend":
				return new string[] {
					"A_bottle_1",
					"A_bottle_2",
					"A_cup_1",
					"A_cup_2",
					"A_mixing_cap_1",
					"A_mixing_cap_2",
					"A_mixing_cup_1",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_bottle_3",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_bottle_4",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cup_4",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_mixing_cap_4",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_cup_3",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_mixing_cap_3",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_mixing_cup_2",
				};
			case "stand_smoke":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_smoke_1"
				};
			case "stand_experiment_chemistry":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cuvette_1"
				};
			case "stand_vr":
				return new string[] {
					"A_vr_glass_1",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/handle"
				};
			case "stand_vr_handle":
				return new string[] {
					"A_vr_glass_1",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/handle"
				};
			// --
			case "sit_swing_with_music":
				return new string[] {
					"A_headset_1"
				};
			case "sit_computer":
				return new string[] {
					"A_mouse_1"
				};
			case "sit_keyboard":
				return new string[] {
					"A_mouse_1"
				};
			case "sit_mouse_move":
				return new string[] {
					"A_mouse_1"
				};
			case "sit_mouse_click":
				return new string[] {
					"A_mouse_1"
				};
			case "sit_scrath_head_computer":
				return new string[] {
					"A_mouse_1"
				};
			case "sit_chin_in_hand_computer":
				return new string[] {
					"A_mouse_1"
				};
			case "sit_hold_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "sit_play_cellphone_click":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "sit_play_cellphone_scroll":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "sit_photo_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_1"
				};
			case "sit_talk_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_3"
				};
			case "sit_listen_cellphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cellphone_3"
				};
			case "sit_ipad":
				return new string[] {
					"A_ipad_1"
				};
			case "sit_ipad_click":
				return new string[] {
					"A_ipad_1"
				};
			case "sit_ipad_scroll":
				return new string[] {
					"A_ipad_1"
				};
			case "sit_write_pen":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pen_3"
				};
			case "sit_write_dictate":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pen_4",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_paper_3"
				};
			case "sit_draw_pen":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pen_5"
				};
			case "sit_crumple_paper":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_crumple_paper_1"
				};
			case "sit_recorder":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pen_6"
				};
			case "sit_vr":
				return new string[] {
					"A_vr_glass_1",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_vr_handle_1"
				};
			case "sit_vr_handle":
				return new string[] {
					"A_vr_glass_1",
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_vr_handle_1"
				};
			case "sit_drink_coffee":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cup_5"
				};
			case "sit_solder":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_solder_L_2",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_solder_R_2"
				};
			case "sit_fan":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_fan_1"
				};
			case "sit_drink_water":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cup_5"
				};
			case "sit_with_tableware":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_knife_1",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_fork_1"
				};
			case "sit_eat_with_tableware":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_knife_1",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_fork_1"
				};
			case "sit_talk_with_tableware":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_knife_1",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_fork_1"
				};
			case "sit_mixer":
				return new string[] {
					"A_headset_1"
				};
			case "sit_draw_digital_3d_pen":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pen_7"
				};
			case "sit_remote":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_remote_control_2"
				};
			case "sit_spin_pen":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_thumbtip_Goal/A_pen_8"
				};
			case "sit_calculator":
				return new string[] {
					"A_computer_1",
					"A_mouse_2"
				};
			case "sit_experiment":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_cuvette_L_1",
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_cuvette_R_1"
				};
			case "sit_snacks":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/right_hand_Goal/A_snacks_1"
				};
			case "sit_draw_digital_pen":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/A_pen_8"
				};
			case "sit_mic_self":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_thumb/A_mic_2"
				};
			case "sit_mic_other":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/left_thumb/A_mic_2"
				};

			// B - gain from scene (object under such path should be hidden, but used as a target to be followed by object in scene)
			case "stand_fixphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/B_fixphone_1"
				};
			case "stand_fixphone_dial":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/B_fixphone_1"
				};
			case "stand_wine":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/B_cup_1"
				};
			case "stand_wine_toast":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/B_cup_1"
				};

			case "sit_fixphone":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/B_fixphone_1"
				};
			case "sit_fixphone_dial":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_arm/left_elbow/left_hand/B_fixphone_1"
				};
			case "sit_wine":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/B_cup_1"
				};
			case "sit_wine_toast":
				return new string[] {
					"hip_ctrl/root/spline/right_chest/left_hand_Goal/B_cup_1"
				};

			case "other_exercise_1":
				return new string[] {
					"B_barbell_1"
				};
			case "other_exercise_2":
				return new string[] {
					"B_dumbbell_1",
					"B_dumbbell_2"
				};
			case "other_exercise_3":
				return new string[] {
					"B_dumbbell_1",
					"B_dumbbell_2"
				};


			// C - create in scene (object under such path should be hidden, but used as a target to be followed by object in scene)
			case "stand_poster":
				return new string[] {
					"C_poster_1"
				};
			case "stand_stickynote_wall":
				return new string[] {
					"C_stickynote_1"
				};

			default:
				return null;
			}
		}

		// 返回动作需要创建的物品名称
		public static string FindPrefabs (string stateName)
		{
			switch (stateName) {
			case "stand_poster":
				return "poster";
			case "stand_stickynote_wall":
				return "stickynote";
			default:
				return null;
			}
		}

		public static StuffType FindStuffType (string stateName)
		{
			switch (stateName) {
			case "stand_small_pickup_table":
				return StuffType.SmallStuff;
			case "stand_small_pickup_ground":
				return StuffType.SmallStuff;
			case "stand_middle_pickup_table":
				return StuffType.MiddleStuff;
			case "stand_middle_pickup_ground":
				return StuffType.MiddleStuff;
			case "stand_large_pickup_table_front":
				return StuffType.BigStuff;
			case "stand_large_pickup_ground_above":
				return StuffType.BigStuff;
			case "stand_paper_pickup_table":
				return StuffType.PaperStuff;
			case "stand_largepaper_pickup_table":
				return StuffType.LargepaperStuff;
			case "stand_stick_pickup_table":
				return StuffType.StickStuff;
			case "stand_book_pickup_table":
				return StuffType.BookStuff;
			case "stand_book_pickup_shelf":
				return StuffType.BookStuff;
			default:
				return StuffType.SmallStuff;
			}
		}
	}

	public enum ActionType
	{
		error,
		sit,
		stand
	}

	public enum SitActionName
	{
		sit,
		stand_up,
		sit_head_left30,
		sit_head_left90,
		sit_head_right30,
		sit_head_right90,
		sit_breathe,
		sit_shake_leg,
		sit_sprawl,
		sit_look_up,
		sit_relax_1,
		sit_relax_2,
		sit_left30,
		sit_right30,
		sit_nod,
		sit_shake_head,
		sit_cross_arm,
		sit_cross_arm_shake_head,
		sit_scratch_head,
		sit_chin_in_hand,
		sit_scratch_head_computer,
		sit_chin_in_hand_computer,
		sit_sleep,
		sit_raise_righthand,
		sit_hello,
		sit_watch,
		sit_applaud,
		sit_knock_table,
		sit_talk,
		sit_talk_point_ppt,
		sit_talk_point_near,
		sit_talk_point_far,
		sit_talk_look_up,
		sit_laugh_loud,
		sit_backward,
		sit_hold_head_back,
		sit_swing_with_music,
		sit_computer,
		sit_keyboard,
		sit_mouse_move,
		sit_mouse_click,
		sit_fixphone,
		sit_fixphone_pickup,
		sit_fixphone_dial,
		sit_fixphone_putdown,
		sit_hold_cellphone,
		sit_play_cellphone_scroll,
		sit_play_cellphone_click,
		sit_play_cellphone_pickup,
		sit_play_cellphone_putdown,
		sit_photo_cellphone,
		sit_talk_cellphone,
		sit_talk_cellphone_pickup,
		sit_talk_cellphone_putdown,
		sit_listen_cellphone,
		sit_ipad,
		sit_ipad_pickup,
		sit_ipad_putdown,
		sit_ipad_click,
		sit_ipad_scroll,
		sit_write_pen,
		sit_write_dictate,
		sit_spin_pen,
		sit_crumple_paper,
		sit_draw_pen,
		sit_draw_digital_pen,
		sit_draw_digital_3d_pen,
		sit_small_hold,
		sit_small_pickup_table,
		sit_small_pickup_bag,
		sit_small_putdown_table,
		sit_small_putdown_bag,
		sit_small_observe,
		sit_small_point,
		sit_small_pickup_ground,
		sit_middle_hold,
		sit_middle_pickup_table,
		sit_middle_pickup_bag,
		sit_middle_putdown_table,
		sit_middle_putdown_bag,
		sit_middle_observe,
		sit_middle_point,
		sit_paper_hold_front,
		sit_paper_read,
		sit_paper_pickup_table,
		sit_paper_pickup_bag,
		sit_paper_putdown_table,
		sit_paper_putdown_bag,
		sit_largepaper_read,
		sit_largepaper_pickup_table,
		sit_largepaper_pickup_bag,
		sit_largepaper_putdown_table,
		sit_largepaper_putdown_bag,
		sit_book_hold,
		sit_book_read,
		sit_book_pickup_table,
		sit_book_pickup_bag,
		sit_book_putdown_table,
		sit_book_putdown_bag,
		sit_recorder,
		sit_recorder_pickup,
		sit_recorder_putdown,
		sit_mic_other,
		sit_mic_self,
		sit_vr,
		sit_vr_pickup,
		sit_vr_putdown,
		sit_vr_handle,
		//sit_helmet_pickup,
		//sit_helmet_putdown,
		sit_drink_coffee,
		sit_remote,
		//sit_controller,
		sit_calculator,
		sit_experiment,
		sit_solder,
		sit_sewing_machine,
		//sit_piano,
		//sit_guitar,
		sit_drive,
		sit_snacks,
		sit_fan,
		sit_mixer,
		//sit_motion_sensor,
		//sit_printer,
		//sit_3dprinter_productstoryline,
		//sit_3dprinter_architectstoryline,
		//sit_cnc_productstoryline,
		//sit_holo,
		//sit_panel,
		//sit_experiment_chemistry,
		//sit_experiment_pioneerstoryline,
		//sit_insert,
		//sit_steel_tape,
		//sit_tailor_tape,
		//sit_tailor_tape_model,
		//sit_cut,
		//sit_iron,
		//sit_saws,
		//sit_grinder,
		//sit_poster,
		//sit_stickynote_wall,
		//sit_write_wall,
		//sit_record_rod,
		//sit_light,
		//sit_mirror_clothes,
		//sit_water_flower,
		sit_drink_water,
		sit_wine,
		sit_wine_pickup,
		sit_wine_putdown,
		sit_wine_toast,
		sit_with_tableware,
		sit_talk_with_tableware,
		sit_eat_with_tableware,
		//sit_serve_dish,
		//sit_take_dish,
		//sit_pour_wine,
		//sit_bartend,
		sit_jump_cheer,
		//sit_xbox,
		//sit_table_soccer,
		//sit_pose_icecream,

		sit_smoke,
		sit_talk_point_near_times,
		sit_talk_point_far_times,
		sit_experiment_pioneerstoryline
	}

	public enum StandActionName
	{
		stand,
		sit_down,
		walk_blend_tree,
		walk_small_blend_tree,
		walk_middle_blend_tree,
		walk_book_blend_tree,
		stand_head_left30,
		stand_head_left90,
		stand_head_right30,
		stand_head_right90,
		stand_breathe,
		stand_look_up,
		stand_left30,
		stand_right30,
		stand_nod,
		stand_shake_head,
		stand_cross_arm,
		stand_cross_arm_shake_head,
		stand_scratch_head,
		stand_chin_in_hand,
		stand_raise_righthand,
		stand_hello,
		stand_watch,
		stand_applaud,
		stand_knock_table,
		stand_talk,
		stand_talk_point_ppt,
		stand_talk_point_near,
		stand_talk_point_far,
		stand_talk_point_near_times,
		stand_talk_point_far_times,
		stand_talk_paper,
		stand_talk_mic,
		stand_talk_megaphone,
		stand_talk_megaphone_point_far_times,
		stand_talk_interphone,
		stand_talk_interphone_point_far_times,
		stand_talk_look_down,
		stand_laugh_loud,
		stand_bow,
		//stand_swing_with_music,
		stand_fixphone,
		stand_fixphone_pickup,
		stand_fixphone_putdown,
		stand_fixphone_dial,
		stand_hold_cellphone,
		stand_play_cellphone_scroll,
		stand_play_cellphone_click,
		stand_play_cellphone_pickup,
		stand_play_cellphone_putdown,
		stand_photo_cellphone,
		stand_talk_cellphone,
		stand_talk_cellphone_pickup,
		stand_talk_cellphone_putdown,
		stand_listen_cellphone,
		stand_ipad,
		stand_ipad_pickup,
		stand_ipad_putdown,
		stand_ipad_click,
		stand_ipad_scroll,
		stand_write_pen,
		stand_small_hold,
		stand_small_pickup_table,
		stand_small_pickup_bag,
		stand_small_putdown_table,
		stand_small_putdown_bag,
		stand_small_observe,
		stand_small_point,
		stand_small_pickup_ground,
		inter_small_give,
		inter_small_take,
		stand_middle_hold,
		stand_middle_pickup_table,
		stand_middle_pickup_bag,
		stand_middle_putdown_table,
		stand_middle_putdown_bag,
		stand_middle_observe,
		stand_middle_point,
		stand_middle_pickup_ground,
		inter_middle_give,
		inter_middle_take,
		stand_large_hold_front,
		stand_large_pickup_table_front,
		stand_large_putdown_table_front,
		stand_large_hold_above,
		stand_large_pickup_ground_above,
		stand_large_putdown_ground_above,
		stand_paper_hold_front,
		stand_paper_read,
		stand_paper_pickup_table,
		stand_paper_pickup_bag,
		stand_paper_putdown_table,
		stand_paper_putdown_bag,
		//inter_paper_give,
		//inter_paper_take,
		stand_largepaper_read,
		stand_largepaper_pickup_table,
		stand_largepaper_pickup_bag,
		stand_largepaper_putdown_table,
		stand_largepaper_putdown_bag,
		stand_stick_hold,
		stand_stick_pickup_table,
		stand_stick_putdown_table,
		stand_book_hold,
		stand_book_read,
		stand_book_pickup_table,
		stand_book_pickup_bag,
		stand_book_putdown_table,
		stand_book_putdown_bag,
		stand_book_pickup_shelf,
		stand_book_putdown_shelf,
		inter_book_give,
		inter_book_take,
		stand_recorder,
		stand_recorder_pickup,
		stand_recorder_putdown,
		stand_camera,
		stand_camera_pickup,
		stand_camera_putdown,
		stand_mic_other,
		stand_mic_pickup,
		stand_mic_putdown,
		stand_mic_self,
		stand_vr,
		stand_vr_handle,
		//stand_helmet_pickup,
		//stand_helmet_putdown,
		stand_remote,
		stand_controller,
		stand_solder,
		stand_sewing_machine,
		stand_scratch_cd,
		stand_printer,
		stand_3dprinter_productstoryline,
		stand_3dprinter_architectstoryline,
		stand_cnc_productstoryline,
		stand_holo,
		stand_panel,
		stand_experiment,
		stand_experiment_chemistry,
		stand_experiment_pioneerstoryline,
		stand_insert,
		stand_steel_tape,
		stand_tailor_tape,
		stand_tailor_tape_model,
		stand_cut,
		stand_iron,
		stand_saws,
		stand_grinder,
		stand_poster,
		stand_stickynote_wall,
		stand_write_wall,
		stand_record_rod,
		stand_light,
		stand_mirror_clothes,
		stand_water_flower,
		stand_drink_water,
		stand_wine,
		stand_wine_toast,
		stand_serve_dish,
		stand_take_dish,
		stand_pour_wine,
		stand_bartend,
		stand_jump_cheer,
		stand_xbox,
		stand_table_soccer,
		//stand_pose_icecream,

		stand_smoke,
		stand_pipe,
		stand_motion_sensor,
		stand_yawn,
		stand_wave_hand,
		stand_computer,
		stand_keyboard,
		stand_mouse_move,
		stand_mouse_click,
		stand_scratch_head_computer,

		inter_shake_hand,
		other_hug,
		inter_cheers,
		//other_get_on_car,
		//other_get_off_car,
		other_exercise_1,
		other_exercise_2,
		other_exercise_3,
		other_lay,
		other_ladder_up,
		other_ladder_down,
		other_ladder_operate,
		other_sit_table,
		other_dance_1,
		other_dance_2,
		other_dance_3,
		other_play_slide,
		other_run,
		drag_float
	}

	public enum SitActionWithBorder
	{
		sit_head_left30,
		sit_head_left90,
		sit_head_right30,
		sit_head_right90,
		sit_breathe,
		sit_shake_leg,
		sit_sprawl,
		sit_look_up,
		sit_relax_2,
		sit_left30,
		sit_right30,
		sit_cross_arm,
		sit_cross_arm_shake_head,
		sit_sleep,
		sit_raise_righthand,
		sit_applaud,
		sit_talk_look_up,
		sit_backward,
		sit_hold_head_back,
		sit_computer,
		sit_keyboard,
		sit_mouse_move,
		sit_mouse_click,
		sit_scratch_head_computer,
		sit_chin_in_hand_computer,
		sit_fixphone,
		sit_fixphone_dial,
		sit_hold_cellphone,
		sit_play_cellphone_click,
		sit_play_cellphone_scroll,
		sit_talk_cellphone,
		sit_listen_cellphone,
		sit_ipad,
		sit_ipad_click,
		sit_ipad_scroll,
		sit_write_pen,
		sit_write_dictate,
		sit_spin_pen,
		sit_draw_pen,
		sit_draw_digital_pen,
		sit_recorder,
		sit_mic_other,
		sit_mic_self,
		sit_vr,
		sit_vr_handle,
		sit_remote,
		sit_calculator,
		sit_fan,
		sit_mixer,
		sit_wine,
		sit_wine_toast,
		sit_with_tableware,
		sit_eat_with_tableware,
		sit_talk_with_tableware,

		sit_drive,
		sit_smoke
	}

	public enum StandActionWithBorder
	{
		stand_head_left30,
		stand_head_left90,
		stand_head_right30,
		stand_head_right90,
		stand_look_up,
		stand_left30,
		stand_right30,
		stand_cross_arm,
		stand_cross_arm_shake_head,
		stand_chin_in_hand,
		stand_raise_righthand,
		stand_applaud,
		stand_knock_table,
		stand_talk_look_down,
		stand_fixphone,
		stand_fixphone_dial,
		stand_hold_cellphone,
		stand_play_cellphone_click,
		stand_play_cellphone_scroll,
		stand_talk_cellphone,
		stand_listen_cellphone,
		stand_ipad,
		stand_ipad_click,
		stand_ipad_scroll,
		stand_write_pen,
		stand_recorder,
		stand_camera,
		stand_mic_other,
		stand_mic_self,
		stand_remote,
		stand_controller,
		stand_scratch_cd,
		stand_record_rod,
		stand_light,
		stand_mirror_clothes,
		stand_wine,
		stand_wine_toast,
		stand_vr,
		stand_vr_handle,
		stand_computer,
		stand_keyboard,
		stand_mouse_move,
		stand_mouse_click,
		stand_scratch_head_computer,
		stand_love,

		stand_smoke,
		inter_shake_hand,
		other_exercise_2,
		other_exercise_3,
		other_lay,
		other_sit_table,
		other_run,
		drag_float
	}
}
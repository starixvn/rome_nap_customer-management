/* jshint strict: false,
camelcase: false */
/* exported keyboardManager */

var KeyboardManager = function () {
    var self = this;
    self.Enums = {
        KeyCode: {
            None: 0,
            Backspace: 8,
            Tab: 9,
            Enter: 13,
            Shift: 16,
            Ctrl: 17,
            Escape: 27,
            Alt: 18,
            End: 35,
            Home: 36,
            LeftArrow: 37,
            UpArrow: 38,
            RightArrow: 39,
            DownArrow: 40,
            Delete: 46,
            Number_0: 48,
            Number_9: 57,
            Character_a: 65,
            Character_b: 66,
            Character_c: 67,
            Character_d: 68,
            Character_e: 69,
            Character_f: 70,
            Character_g: 71,
            Character_h: 72,
            Character_i: 73,
            Character_j: 74,
            Character_k: 75,
            Character_l: 76,
            Character_m: 77,
            Character_n: 78,
            Character_o: 79,
            Character_p: 80,
            Character_q: 81,
            Character_r: 82,
            Character_s: 83,
            Character_t: 84,
            Character_u: 85,
            Character_v: 86,
            Character_w: 87,
            Character_x: 88,
            Character_y: 89,
            Character_z: 90,
            NumberPad_0: 96,
            NumberPad_9: 105,

            // Character ,
            Character_Comma: 188,
            // Character .
            Character_Period: 110,
            Character_Period1: 190
        }
    };
};

var keyboardManager = new KeyboardManager();
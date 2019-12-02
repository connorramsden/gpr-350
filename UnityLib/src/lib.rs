#![allow(non_snake_case, dead_code)]

extern crate cgmath;

use std::os::raw::{c_int, c_double};
use cgmath::{Vector3, InnerSpace};

#[repr(C)]
pub struct SphereHull {
    radius: f64,
    center: Vector3<f64>
}

impl SphereHull {
    pub fn check_vs_sphere(self, other: SphereHull) -> bool {
        let distance: Vector3<f64> = self.center - other.center;
        let dist_squared = distance.dot(distance);

        false
    }
}

// TESTING STUFF FOR REFERENCE //
#[repr(C)]
pub struct TestStruct {
    val: i32,
    pos: Vector3<f64>,
}

#[no_mangle]
pub extern "C" fn testFunc(args: &mut TestStruct) -> c_int {
    // args.val.add_assign(10);

    args.val
}

#[no_mangle]
pub extern "C" fn testFuncTwo(args: &mut TestStruct) -> c_double {
    // args.pos.x.add_assign(1.0);

    args.pos.x
}
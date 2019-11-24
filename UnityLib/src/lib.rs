#![allow(unused)]
extern crate prost;
extern crate prost_build;
extern crate prost_derive;

use std::os::raw::{c_int, c_char};
use std::ffi::CStr;
use std::borrow::Borrow;

#[derive(Clone, Debug, PartialEq)]
struct CollisionHull {
    vs_sphere: bool,
    vs_aabb: bool,
    vs_obb: bool,
    is_coll: bool,
    min: Vec<f64>,
    max: Vec<f64>,
}

fn deserialize_hull(data: &str) -> CollisionHull {
    let vec: Vec<f64> = Vec::new();
    
    let v: CollisionHull = CollisionHull {vs_sphere: false, vs_aabb: false, vs_obb: false, is_coll: false, min: Vec::from(vec.borrow()), max: Vec::from(vec.borrow()) };

    v
}

unsafe fn to_owned(unowned: *const c_char) -> String {
    CStr::from_ptr(unowned).to_string_lossy().into_owned()
}

/// # Safety
/// This function is being called  from C#.
/// Don't expect the usual Rust safety, but at least some!
#[no_mangle] // Ensures that we can call this from C#
pub unsafe fn test_bool(foreign_data: *const c_char) -> c_int {
    // Convert foreign_data into a Rust-safe string
    let local_data: String = to_owned(foreign_data);

    // Lend the local_data to deserialize_hull
    let hull: CollisionHull = deserialize_hull(local_data.borrow());

    if hull.is_coll {
        0
    } else {
        1
    }
}
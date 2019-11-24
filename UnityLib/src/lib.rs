extern crate prost;
extern crate prost_derive;

use std::os::raw::c_char;
use std::os::raw::c_int;

pub mod hulls {
    include!(concat!(env!("OUT_DIR"), "/phys.hulls.rs"));
}

/// # Safety
/// This function is being called  from C#.
/// Don't expect the usual Rust safety, but at least some!
#[no_mangle] // Ensures that we can call this from C#
pub fn check_collisions(_foreign_data: *const c_char) -> c_int {
    0
}

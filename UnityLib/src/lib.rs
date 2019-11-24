extern crate bytes;
extern crate prost;
extern crate prost_derive;
extern crate ffi_support;

use std::os::raw::c_int;
use ffi_support::{FfiStr};

pub mod phys {
    include!(concat!(env!("OUT_DIR"), "/phys.rs"));
}

#[no_mangle] // Ensures that we can call this from C#
pub extern "C" fn check_collisions(foreign_hull: FfiStr) -> c_int {

    0
}

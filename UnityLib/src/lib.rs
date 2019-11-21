extern crate rayon;
extern crate serde_json;

use std::fs::{File};
use std::io::{BufReader};
use std::os::raw::{c_int, c_char};
use std::ffi::{CStr};

use serde_derive::{Deserialize, Serialize};

#[derive(Serialize, Deserialize)]
struct CollisionHull {
    vs_sphere: bool,
    vs_aabb: bool,
    vs_obb: bool,
    is_coll: bool
}

fn deserialize_hull(path: *const c_char) -> String {
    let local_path;
    unsafe { local_path = CStr::from_ptr(path).to_string_lossy().into_owned(); }

    if local_path == "Cube.json"{
        return local_path;
    }

    let file = File::open(local_path);
    let buf_reader: BufReader<File> = BufReader::new(file.unwrap());

    let some_hull: CollisionHull = serde_json::from_reader(buf_reader).unwrap();

    return String::new();
}

#[no_mangle]
pub fn test_bool(path: *const c_char) -> c_int {
    let some_result = deserialize_hull(path);

    if some_result == "Cube.json" {
        return 0;
    } else {
        return 1;
    }
}
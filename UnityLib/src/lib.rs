extern crate rayon;
extern crate serde_json;

use std::fs::File;
use std::io::Read;
use std::os::raw::c_int;
use std::ffi::{CStr, CString};

use rayon::prelude::*;

#[no_mangle]
pub fn deserialize_hull(path: CString) -> std::io::Result<String> {
    let mut file = File::open(path.to_str().expect("Unable to find desired path"))?;
    let mut contents = String::new();

    file.read_to_string(&mut contents)?;

    Ok(contents)
}

#[no_mangle]
pub fn test_bool(path: CString) -> c_int {
    let some_result = deserialize_hull(path);

    if some_result.is_ok() {
        return 0;
    } else {
        return 1;
    }
}
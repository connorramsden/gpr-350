extern crate rayon;
extern crate serde_json;

use std::fs::File;
use std::io::Read;
use std::os::raw::c_int;

#[no_mangle]
pub fn deserialize_hull() -> std::io::Result<String> {
    let mut file = File::open("../../Cube.json")?;
    let mut contents = String::new();

    file.read_to_string(&mut contents)?;

    Ok(contents)
}

#[no_mangle]
pub fn test_bool() -> c_int {
    let some_result = deserialize_hull();

    if some_result.is_ok() {
        return 0;
    } else {
        return 1;
    }
}
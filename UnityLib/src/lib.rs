#[no_mangle]
pub fn deserialize_hull() {
    let parsed = json::parse("Cube.json").unwrap();
}
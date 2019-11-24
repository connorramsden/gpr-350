fn main() {
    prost_build::compile_protos(&["src/hulls.proto"], 
                                &["src/"]).unwrap();
}

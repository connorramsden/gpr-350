fn main() {
    prost_build::compile_protos(
        &["../PhysicsLabFramework/Assets/Editor/hulls.proto"],
        &["../PhysicsLabFramework/Assets/Editor/"],
    )
    .unwrap();
}

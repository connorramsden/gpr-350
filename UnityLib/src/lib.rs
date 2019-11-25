extern crate bytes;
extern crate ffi_support;
extern crate prost;
extern crate prost_derive;

use ffi_support::ExternError;

pub mod phys {
	include!(concat!(env!("OUT_DIR"), "/phys.rs"));
}

/// # Safety
/// This is dealing with pointers, and its unsafe!
/// Don't expect usual Rust safety coming into this function
#[no_mangle] // Ensures that we can call this from C#
pub unsafe extern "C" fn ulib_check_collisions(data: *const u8, len: i32, error: &mut ExternError) {
	ffi_support::call_with_result(error, || {
		let bytes = if len == 0 {
			&[]
		} else {
			assert!(!data.is_null(), "Unexpected null data pointer");
			std::slice::from_raw_parts(data, len as usize)
		};

		Ok(())
	})
}

# 0008 Seller profile editing

- Owners can edit their existing profile with visible file inputs and a responsive layout. Other seller users cannot read or submit the owner form.
- Store work samples and contact links use coordinated table styling on desktop and compact card rows on small screens. Add and delete actions retain their existing routes and handlers, with accessible labels and empty states.
- Create work samples from the profile page modal; the standalone AddWorkSample page is not available. The modal uses the existing owner-scoped save flow, validates the uploaded image, and sends antiforgery tokens on upload and save.
- Invalid text, location hierarchy or replacement files produce feedback without saving. Omitted files retain the existing images. Invalid submissions retain entered text.
- Replacement images are written before changing the database reference. Existing images are removed after a successful save. A successful edit returns both the seller profile and its market to review, including previously approved stores.


## Evidence

The Edge browser runner passed profile layout checks at 320, 390 and 1440px, responsive sample/link rows, add/delete controls, sample modal open/close and absence of the standalone route, rejection and retry for missing city, fixture-backed save, employee denial, antiforgery, and image-error feedback. An authenticated local request with the real service rejected a malformed PNG and retained all three stored image references.

- Saving seller personal information, an introduction video, or received-cheque settings requests support review for the seller's store.
- An expired active subscription deactivates both the market and its seller-profile state, regardless of elapsed duration. The next charge check also repairs legacy completion states from expired charge history. It does not relabel retained profile data as incomplete or delete it.
- The shared seller status maps the legacy waiting-for-completion state consistently on dashboards and other seller pages.

The Edge browser runner passed the video and cheque post-save review-transition checks and verified that the legacy completion state has the same short label on dashboard and secondary seller pages.

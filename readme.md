### For all
- create unit tests
- make sure it shows up on zipkin with attributes.


1. Create a new PUT method on the UpdateOffer controller "OfferSent"
- it should update the offer status to OfferSent
- it should send out an updatedOffer event.


2. Create a new Feature OfferAccepted
- it will have it's own event topic called "offer.accepted"
- update the offer with the offerstatus of OfferAccepted
- fire off your new event

3. Create your own micro service called NewEmployee
- it subscribes to the "offer.accepted"
- it could multiple of things
   - send off a "newemployee.adaccountedneeded"
   - send off an "newemployee.created"
- add method ADAccountCreated.
    - the logic app (or other microservice) will listen to the "newemployee.adaccountedneeded" create the AD and call this event (providing the AD username)
    - update your state with AD username
    - send out an event that "newemployee.ADAccountActived"

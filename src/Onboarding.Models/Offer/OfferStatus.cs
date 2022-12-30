namespace Onboarding.Models.Offer;

public enum OfferStatus
{
    New,
    ManagerSignOffRequested,
    ManagerNotApproved,
    AwaitingDocumentCreation,
    AwaitingSend,
    OfferSent,
    OfferAccepted,
    Sent,
    Accepted,
    Declined
}
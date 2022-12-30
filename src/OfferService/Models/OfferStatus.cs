namespace OfferService.Models;

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
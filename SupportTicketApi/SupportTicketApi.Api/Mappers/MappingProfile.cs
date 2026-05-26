using AutoMapper;
using SupportTicketApi.Api.DataTransferObjects;
using SupportTicketApi.Api.DataTransferObjects.Activity;
using SupportTicketApi.Api.DataTransferObjects.ActivityType;
using SupportTicketApi.Api.DataTransferObjects.Company;
using SupportTicketApi.Api.DataTransferObjects.Contact;
using SupportTicketApi.Api.DataTransferObjects.Discipline;
using SupportTicketApi.Api.DataTransferObjects.Lead;
using SupportTicketApi.Api.DataTransferObjects.LeadPhase;
using SupportTicketApi.Api.DataTransferObjects.SwecoUser;
using SupportTicketApi.Core.Models;

namespace SupportTicketApi.Api.Mappers;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<ActivityCreateOrReplaceRequest, Activity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ActivityType, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Lead, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore());
        CreateMap<ActivityUpdateRequest, Activity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ActivityType, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Lead, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore());
        CreateMap<Activity, ActivityReadRequest>();

        // NOTE: this reverse mapping simplifies PATCH functionality using JsonPatchDocuments
        CreateMap<Activity, ActivityCreateOrReplaceRequest>();

        CreateMap<ActivityTypeCreateOrReplaceRequest, ActivityType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<ActivityTypeUpdateRequest, ActivityType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<ActivityType, ActivityTypeReadRequest>();

        CreateMap<CompanyCreateOrReplaceRequest, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CompanyType, opt => opt.Ignore());
        CreateMap<CompanyUpdateRequest, Company>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CompanyType, opt => opt.Ignore());
        CreateMap<Company, CompanyReadRequest>();

        // NOTE: this reverse mapping simplifies PATCH functionality using JsonPatchDocuments
        CreateMap<Company, CompanyCreateOrReplaceRequest>();

        CreateMap<CompanyTypeCreateOrReplaceRequest, CompanyType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<CompanyTypeUpdateRequest, CompanyType>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<CompanyType, CompanyTypeReadRequest>();

        CreateMap<ContactCreateOrReplaceRequest, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore());
        CreateMap<ContactUpdateRequest, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore());
        CreateMap<Contact, ContactReadRequest>();

        // NOTE: this reverse mapping simplifies PATCH functionality using JsonPatchDocuments
        CreateMap<Contact, ContactCreateOrReplaceRequest>();

        CreateMap<DisciplineCreateOrReplaceRequest, Discipline>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<DisciplineCreateOrReplaceRequest, Discipline>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Discipline, DisciplineReadRequest>();

        CreateMap<LeadCreateOrReplaceRequest, Lead>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore())
            .ForMember(dest => dest.Disciplines, opt => opt.Ignore())
            .ForMember(dest => dest.LeadPhase, opt => opt.Ignore());
        CreateMap<LeadUpdateRequest, Lead>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore())
            .ForMember(dest => dest.Disciplines, opt => opt.Ignore())
            .ForMember(dest => dest.LeadPhase, opt => opt.Ignore());
        CreateMap<Lead, LeadReadRequest>();

        // NOTE: this reverse mapping simplifies PATCH functionality using JsonPatchDocuments
        CreateMap<Lead, LeadCreateOrReplaceRequest>();

        CreateMap<LeadPhase, LeadPhaseReadRequest>();

        CreateMap<SwecoUser, SwecoUserReadRequest>();
        CreateMap<SwecoUserCreateOrReplaceRequest, SwecoUser>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SwecoUserRoles, opt => opt.Ignore());
        CreateMap<SwecoUserRole, SwecoUserRoleReadRequest>();

        // NOTE: this reverse mapping simplifies PATCH functionality using JsonPatchDocuments
        CreateMap<SwecoUser, SwecoUserCreateOrReplaceRequest>();
    }
}

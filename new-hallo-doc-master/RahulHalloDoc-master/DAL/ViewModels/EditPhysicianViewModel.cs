using DAL.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class EditPhysicianViewModel
    {
        public string? PhysicianUsername {  get; set; }
        public string? PhysicianPassword { get; set; }
        public string? ResidentialRegion {  get; set; }
        public short? Status {  get; set; }
        public List<Role>? Role {  get; set; }
        public int? RoleId {  get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? Email {  get; set; }
        public string? PhoneNo {  get; set; }
        public string? MedicalLicense {  get; set; }
        public string? NPINumber {  get; set; }
        public string? SyncEmail {  get; set; }
        public List<Region>? States { get; set; }
        public int PhysicianId {  get; set; }
        public string? Address1 {  get; set; }
        public string? Address2 {  get; set; }
        public List<string>? ListOfServicingStates {  get; set; }
        public string? City {  get; set; }
        public string? ZipCode {  get; set; }
        public string? BillingPhoneNo {  get; set; }
        public string? BusinessName {  get; set; }
        public string? BusinessWebsite {  get; set; }
        public string? AdminNotes {  get; set; }
        public int? Regionid {  get; set; }
        public IFormFile? SelectPhoto {  get; set; }
        public IFormFile? SelectSignature {  get; set; }
        public IFormFile? IndependentContractAgreement {  get; set; }
        public IFormFile? BackgroundCheck { get; set; }
        public IFormFile? HIPAACompliance { get; set; }
        public IFormFile? NonDisclosureAgreement { get; set; }
        public IFormFile? LicenseDocument { get; set; }

        public List<int> SelectedRegions {  get; set; }

        public int? PhysicianRole {  get; set; }
        public int? PhysicianState {  get; set; }
        public bool? ICAexists {  get; set; }
        public bool? BCexists {  get; set; }
        public bool? HIPAAexists {  get; set; }
        public bool? NDCexists {  get; set; }
        public bool? LDexists {  get; set; }
        public string? Signatureexists {  get; set; }
        



    }
}

# Awnings of Ireland — System User Manual

## Table of Contents

1. [Getting Started](#1-getting-started)
2. [Customers](#2-customers)
3. [Workflows](#3-workflows)
4. [Initial Enquiries](#4-initial-enquiries)
5. [Quotes](#5-quotes)
6. [Site Visits](#6-site-visits)
7. [Invoices & Payments](#7-invoices--payments)
8. [Follow-Ups](#8-follow-ups)
9. [Email Inbox (Automatic Processing)](#9-email-inbox-automatic-processing)
10. [Audit Log](#10-audit-log)

---

## 1. Getting Started

### Logging In

1. Open the browser and go to the system URL
2. Enter your **username** and **password**
3. Click **Login**

Your session lasts **24 hours**. You will be automatically logged out after that and need to log in again.

### Logging Out

Click your profile icon in the top right → **Logout**

### Changing Your Password

1. Click your profile icon → **Change Password**
2. Enter your current password
3. Enter and confirm your new password
4. Click **Save**

---

## 2. Customers

Customers are companies or individuals that you manage awnings sales for. Every workflow must be linked to a customer.

### Viewing Customers

- Go to the **Customers** section from the main menu
- The list shows company name, contact name, email, phone, and assigned salesperson

### Adding a New Customer

1. Click **Add Customer**
2. Fill in the company details:
   - Company Name
   - Address (Address 1, 2, 3)
   - Email, Phone, Mobile
   - Assigned Salesperson
3. Add at least one **contact person** (first name, last name, email, phone)
4. Click **Save**

### Editing a Customer

1. Find the customer in the list and click on them
2. Update the relevant fields
3. Click **Save**

### Deleting a Customer

- A customer can only be deleted if they have **no active workflows**
- If workflows exist, delete or complete them first

---

## 3. Workflows

A workflow represents a **sales job** for a customer — from the first enquiry through to final payment. Every sale follows this pipeline:

```
Initial Enquiry → Quote → Site Visit → Invoice → Payment
```

### Creating a Workflow

1. Go to **Customers** → select a customer
2. Click **New Workflow**
3. Fill in:
   - Workflow Name (e.g. "Back Garden Awning")
   - Description
   - Product and Product Type
   - Supplier
4. Click **Create**

### Viewing Workflows for a Customer

1. Open a customer record
2. The **Workflows** tab lists all workflows for that customer
3. Click any workflow to open it

### Updating a Workflow

1. Open the workflow
2. Edit the details
3. Click **Save**

### Deleting a Workflow

- A workflow can only be deleted if it has **no active data** (enquiries, quotes, site visits, invoices)
- If any active records exist, the system will show what is blocking the deletion
- Soft-deleted records (deleted enquiries) do not block deletion

---

## 4. Initial Enquiries

An initial enquiry records the first contact from a customer about a job — usually an email, phone call, or showroom visit.

### Adding an Enquiry

1. Open a workflow
2. Go to the **Initial Enquiry** tab
3. Click **Add Enquiry**
4. Enter:
   - Email address of the customer
   - Comments (summary of the enquiry)
5. Click **Save**

> When a new enquiry is added, any open follow-up reminders for that workflow are automatically dismissed — the system treats a new reply as the customer responding.

### Editing an Enquiry

1. Find the enquiry in the list
2. Click **Edit**
3. Update the comments or email
4. Click **Save**

### Deleting an Enquiry

- Click the **Delete** icon next to the enquiry
- The record is **soft deleted** — it is hidden from view but kept in the database for audit purposes
- Deleted enquiries can be viewed in the audit log

---

## 5. Quotes

A quote is a formal price proposal sent to the customer after the initial enquiry.

### Creating a Quote

1. Open a workflow
2. Go to the **Quote** tab
3. Click **New Quote**
4. Fill in:
   - Quote Number
   - Quote Date and Follow-up Date
   - Product details (width, projection, brackets, motors, controls, add-ons)
   - Notes and Terms
5. Click **Save**

The system automatically calculates pricing based on the product configuration selected.

### Product Pricing Options

When building a quote line the following options are available depending on the product:

| Option | Description |
|---|---|
| Width & Projection | Standard sizes with set prices |
| Brackets | Choose bracket type (filtered by arm type) |
| Motors | Optional motorisation |
| Controls | Remote / switch controls |
| Lighting | Cassette lighting add-on |
| Valance Style | Style surcharge |
| Non-Standard RAL Colour | Colour surcharge |
| ShadePlus | Additional shade option |
| Wall Sealing Profile | Profile add-on |
| Heaters | Heater add-on |

### Sending a Quote

1. Open the quote
2. Click **Send** — this generates the quote PDF and sends it to the customer email on record

### Follow-up Date

Each quote has a follow-up date. When that date passes without a response, the system can generate a follow-up reminder automatically (see [Follow-Ups](#8-follow-ups)).

---

## 6. Site Visits

A site visit is scheduled after a quote is accepted to measure or assess the installation location.

### Scheduling a Site Visit

1. Open a workflow
2. Go to the **Site Visit** tab
3. Click **Schedule Site Visit**
4. Enter:
   - Visit date and time
   - Address (if different from customer address)
   - Notes
5. Click **Save**

### Completing a Site Visit

1. Open the site visit record
2. Fill in the outcome notes
3. Click **Mark as Complete**

---

## 7. Invoices & Payments

### Creating an Invoice

1. Open a workflow
2. Go to the **Invoice** tab
3. Click **Create Invoice**
4. The invoice is pre-populated from the accepted quote
5. Review and adjust if needed
6. Click **Save**

### Recording a Payment

1. Open an invoice
2. Click **Add Payment**
3. Enter:
   - Payment amount
   - Payment date
   - Payment method
4. Click **Save**

Payments are tracked against the invoice total. The invoice is marked **Paid** when fully settled.

### Payment Schedules

For large jobs, payments can be split into a schedule:

1. Open an invoice
2. Click **Payment Schedule**
3. Add instalment amounts and due dates
4. Click **Save**

---

## 8. Follow-Ups

Follow-ups are automatic reminders generated by the system when a customer has not responded to an enquiry within a set number of days.

### How Follow-Ups Are Generated

The system checks all open initial enquiries. If an enquiry is:
- More than **2 days old**
- The workflow has **no quote** yet
- There is **no existing follow-up** for that enquiry

A follow-up reminder is automatically created.

### Viewing Follow-Ups

- Go to **Follow-Ups** from the main menu
- The list shows all active (non-dismissed) follow-ups ordered by most overdue first
- Each entry shows the customer, workflow, and the date of the last enquiry

### Dismissing a Follow-Up

When you have contacted the customer and no further chasing is needed:

1. Click **Dismiss** next to the follow-up
2. Enter a note (e.g. "Called customer — sending revised quote")
3. Click **Confirm**

### Automatic Dismissal

A follow-up is **automatically dismissed** when:
- A new enquiry is added to the same workflow (customer has replied)
- The system sets the dismiss reason as **"Replied"**

---

## 9. Email Inbox (Automatic Processing)

The system monitors a dedicated email inbox and automatically processes incoming emails.

### What Happens When an Email Arrives

1. The email is received in the monitored mailbox
2. The system reads and analyses the email using AI
3. The email is categorised into one of:

| Category | What it means |
|---|---|
| **Initial Enquiry** | New customer enquiry — creates a task and links to workflow if customer is known |
| **Quote Creation** | Customer requesting a quote |
| **Site Visit / Meeting** | Request to schedule a visit |
| **Invoice Due** | Payment-related email |
| **Showroom Booking** | Booking request |
| **Complaint** | Complaint — flagged as Urgent priority |
| **Junk / General** | Spam or unrelated email — no task created |

4. A **task** is created in the system for non-junk emails
5. If the sender email matches an existing customer, the task is **automatically linked** to that customer
6. If the customer has only one active workflow, the task is **automatically completed** and linked to that workflow

### Viewing Processed Emails

- Go to **Email Inbox** from the main menu
- View all processed emails, their category, and linked tasks

### Email Task Management

For emails that were not automatically matched:

1. Open the task from the inbox
2. Manually search and link to the correct customer
3. Link to the relevant workflow
4. Mark as complete

### If Emails Stop Arriving

If you notice new emails are not appearing in the system:

1. Go to **Email Watch** → **Status** to check if the subscription is active
2. If inactive, click **Subscribe** to re-establish the connection
3. The system automatically renews the subscription every hour — manual action is only needed if the connection has dropped

---

## 10. Audit Log

Every change to key records (customers, contacts, workflows, quotes, invoices, site visits) is logged automatically.

### Viewing the Audit Log

1. Go to **Audit Log** from the main menu
2. Filter by:
   - Record type (Customer, Workflow, Quote, etc.)
   - Date range
   - User who made the change
3. Each entry shows the field that changed, the old value, and the new value

The audit log is **read-only** — entries cannot be edited or deleted.

---

## Quick Reference

### Sales Pipeline Summary

| Stage | Action | Next Step |
|---|---|---|
| Lead comes in | Add Initial Enquiry to workflow | Prepare quote |
| Quote ready | Create and send Quote | Wait for response |
| Quote accepted | Schedule Site Visit | Confirm measurements |
| Site visit done | Create Invoice | Collect payment |
| Payment received | Record Payment | Job complete |

### Common Issues

| Problem | Solution |
|---|---|
| Cannot delete a workflow | Check for active enquiries, quotes, or invoices and remove them first |
| Follow-up not disappearing | Manually dismiss it or add a new enquiry to auto-dismiss |
| Email not processed | Check Email Watch status and re-subscribe if needed |
| Logged out unexpectedly | Sessions last 24 hours — log in again |
| Quote price not calculating | Ensure width and projection are selected before choosing add-ons |

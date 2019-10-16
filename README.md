# Kentico-MVC-Demo

MVC Demo project. In order for the generator to work correctly the following objects will have to be created/copied from the portal engine site:

- Users: 

- Page types: 
    * Code name: CMS MenuItem
    * Displayname: Page (menu item)
    * url pattern: /{%DocumentCulture%}/Page/{%NodeAlias%}
    * Fields:
      * MenuItemName : text : 450 size

- Pages:
    * /Products/Coffees/Ethiopia-Yirgacheffe (this exists in default mvc example site)
    * /Campaign-assets/Cafe-promotion/Thank-you (page type: Page (menu item))
    * /Campaign-assets/Cafe-promotion/Colombia (page type: Page (menu item))
    * /Campaign-assets/Cafe-promotion/America-s-coffee-poster (page type: Page (menu item))
    
- Newsletters:
    * Coffee101
    * DancingGoatNewsletter
    * CoffeeClubMembership
    * ColombiaCoffeeSamplePromotion (campaign)
    * ColombiaCoffeePromotion (campaign)
    
- Contact groups:
    * CoffeeClubMembershipRecipients
    * AllContactsWithEmail
    * AllChicagoContactsWithEmail
    
- Forms:
    * ContactUs
      * Form display name: Send us a message
      * CodeName: ContactUs
      * First name: text
      * Last name: text
      * Email address: email - required
      * Message: text area - required
      * After submit: 'Thanks! We'll get back to you soon.'
  
    * TryAFreeSample
      * Form display name: Try a free sample
      * CodeName: TryAFreeSample
      * First name: text - required
      * Last name: text - required
      * Email address: email - required
      * Address: text - required
      * City: text - required
      * Zip: text - required
      * Country: dropdown - required
      * State: text
      * After submit: Redirect to url: '~/Colombia/Thank-you'

    * PartnershipApplication
      * Form display name: Partnership application
      * CodeName: BusinessCustomerRegistration
      * Company name : text - required
      * First name : text - required
      * Last name: text - required
      * Phone : text
      * Email : email - required
      * I am interested in (Becoming a wholesale partner) (Becoming a partner café) : options - required
      * After submit: 'We'll contact you within 48 hours to discuss details and explain any question of yours regarding the partnership.'
  

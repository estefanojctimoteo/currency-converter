# Currency converter

It implements a REST API that it's able to make conversions between two currencies
by using conversion fees requested from an external service.

To be able to make a conversion, an user ID is needed.

The API should register each conversion transaction including all the related information
and offer an endpoint where transactions made by an user can be searched.

1. It should be able to make conversions between 4 currencies, at least (BRL, USD, EUR, JPY);
1. Conversions' fees must be obtained from [https://api.exchangeratesapi.io/latest?base=EUR];
1. Conversions transactions must be recorded in a embedded database, including:
    * User ID;
    * Currency from;
    * Value from;
    * Currency to;
    * Conversion fee;
    * Date/time UTC;
1. A successful transaction must return:
    * Transaction ID;
    * User ID;
    * Currency from;
    * Value from;
    * Currency to;
    * Conversion fee;
    * Date/time UTC;
    * Value to;    
 

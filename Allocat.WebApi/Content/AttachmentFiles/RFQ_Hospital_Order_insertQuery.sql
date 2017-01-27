--to know about id
--select * from TissueBank;
--select * from Hospital
--select * from HospitalType
--select * from TissueBank
--select * from TissueBankProduct
--select * from Status
 --
 BEGIN TRANSACTION trans
BEGIN TRY
 Declare 
 @TissueBankProductId	int       =1011												
,@TissueBankId	int				  =28
,@HospitalId	int				  =3
,@Quantity	int					  =1
,@UnitPrice	numeric(18, 2)		  =452
,@LineTotal	numeric(18, 2)		   =452
,@SalesTax	numeric(18, 2)		  =10
,@Total	numeric(18, 2)			  =462
,@NeedByDate	datetime		  ='2016-01-08 10:30:00.000'
,@TissueBankSendByDate	datetime  =null
,@CreatedDate	datetime		  =getdate()
,@CreatedBy	int					  =8
,@LastModifiedDate	datetime	  =getdate()
,@LastModifiedBy	int			  =8
,@StatusId int					  = 1007
,@TransactionNumber	nvarchar(100)	=null
,@OrderDate	datetime				=getdate()
,@OrderLineTotal	numeric(18, 2)	=79
,@PurchaseRequestId	int				=null
,@RFQId					int			=null
,@AlloCATFees	nchar(10)			=5
,@HospitalPoNumber	nvarchar(20)	=null
,@DeclineRemark	nvarchar(200)			=null
,@IsActive	bit						=1
,@HospitalAddressId		int			=1
	
,@insertedProductEntityId int
,@insertedOrderId int

insert into ShippingDetail values(null,@HospitalAddressId,null,null)

declare @ShippingDetailId int=(select @@IDENTITY);

insert into ProductEntity
(
 TissueBankProductId,TissueBankId,HospitalId
,Quantity,UnitPrice,LineTotal,SalesTax,Total,NeedByDate,TissueBankSendByDate,ShippingDetailId
,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy
)
values
(
 @TissueBankProductId,@TissueBankId,@HospitalId
,@Quantity,@UnitPrice,@LineTotal,@SalesTax,@Total,@NeedByDate,@TissueBankSendByDate,@ShippingDetailId
,@CreatedDate,@CreatedBy,@LastModifiedDate,@LastModifiedBy
)

set @insertedProductEntityId =(select @@IDENTITY);

insert into dbo.[Order]
(
 TransactionNumber,OrderDate,OrderLineTotal,PurchaseRequestId,RFQId,AlloCATFees,HospitalPoNumber,StatusId,DeclineRemark
,IsActive,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy
)
values
(
 @TransactionNumber,@OrderDate,@OrderLineTotal,@PurchaseRequestId,@RFQId,@AlloCATFees,@HospitalPoNumber,@StatusId,@DeclineRemark
,@IsActive,@CreatedDate,@CreatedBy,@LastModifiedDate,@LastModifiedBy
)

set @insertedOrderId =(select @@IDENTITY);

insert into OrderDetail
(
OrderId,ProductEntityId
,CreatedDate,CreatedBy,LastModifiedDate,LastModifiedBy
)
values
(
@insertedOrderId,@insertedProductEntityId
,@CreatedDate,@CreatedBy,@LastModifiedDate,@LastModifiedBy
)

 BEGIN COMMIT TRANSACTION trans
 END
END TRY
BEGIN CATCH
 print 'Error Occured'
 BEGIN ROLLBACK TRANSACTION trans 
 END
END CATCH 
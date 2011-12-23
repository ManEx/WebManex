var rootUrl = $("#rootUrl").val().toLowerCase();

var $order = {
    reloadStatusGrid: function () {
        jQuery("#orders").GridUnload();
        jQuery("#duedates").GridUnload();
        jQuery("#shipments").GridUnload();
        this.getOrdersByStatus();
    },

    getOrdersByStatus: function () {
        var searchResultGrid = jQuery("#orders").jqGrid({
            url: rootUrl + "Orders/List?status=" + $("#orderStatusDd").val() + "&companyId=" + $("#customerDd").val(),
            datatype: "json",
            cache: false,
            colNames: ['OrderId', 'Part Number', 'Status', 'Rev', 'Description', 'Purch Order', 'SO/RMA', 'SO/RMA #', 'Line no', 'Ord Date', 'Ord Qty', 'Ship Qty', 'Back Ord', 'Rlsd'],
            colModel: [
                { name: 'uniqueln', index: 'uniqueln', hidden: true },
                { name: 'part_no', index: 'part_no', editable: false, sortable: true },
                { name: 'status', index: 'status', width: 80, editable: false, sortable: true },
                { name: 'Revision', index: 'Revision', width: 40, editable: false, sortable: true },
                { name: 'Descript', index: 'Descript', width: 310, editable: false, sortable: true },
                { name: 'Pono', index: 'Pono', width: 140, editable: false, sortable: true },
                { name: 'is_rma', index: 'is_rma', width: 65, editable: false, sortable: true, align: "center", formatter: "rmaCheck" },
                { name: 'Sono', index: 'Sono', width: 120, editable: false, sortable: true },
                { name: 'line_no', index: 'line_no', width: 120, editable: false, sortable: false },
   		        { name: 'orderdate', index: 'orderdate', width: 120, editable: false, sortable: true, sorttype: "date", formatter: "date", formatoptions: { srcformat: 'm/d/Y', newformat: 'm/d/Y' }, align: 'right' },
                { name: 'ord_qty', index: 'ord_qty', width: 80, editable: false, sortable: true, sorttype: 'number', align: 'right' },
                { name: 'shippedqty', index: 'shippedqty', width: 80, editable: false, sortable: true, sorttype: 'number', align: 'right' },
                { name: 'Balance', index: 'Balance', width: 80, editable: false, sortable: true, sorttype: 'number', align: 'right' },
                { name: 'Released', index: 'Released', editable: false, sortable: true, align: "center", edittype: 'checkbox', formatter: "imageBool", width: 40 }
   	        ],
            rowNum: 10,
            rowList: [10, 20, 30],
            height: "100%",
            width: $(window).width() +30,
            pagination: true,
            pager: '#ordersPager',
            viewrecords: true,
            sortname: 'status',
            sortorder: 'desc',
            loadonce: true,
            sortable: true,
            ignoreCase: true,
            caption: "Order List",
            emptyDataText: "No orders found",
            grouping: true,
            groupingView: {
                groupField: ['status'],
                groupColumnShow: [false],
                groupText: ['<b>{0} - {1} Order(s)</b>'],
                groupCollapse: false,
                groupOrder: ['desc']
            },
            onSelectRow: function (id) {
                $order.getOrderDetails(id);
            },
            loadComplete: function () {
                jQuery("#orders").trigger("reloadGrid"); // Call to fix client-side sorting
            }
        });
        jQuery("#orders").jqGrid('navGrid', '#ordersPager', { edit: false, add: false, del: false, refresh: false }, {}, {}, {}, { multipleSearch: true, multipleGroup: false });
        //$("#search_orders").remove();
        $(".ui-jqgrid-titlebar").hide();
        $global.changeUpperRadius('gbox_orders');
        $("#companyName").html($("#customerDd option:selected").text());
    },

    getOrderDetails: function (orderId) {
        jQuery("#duedates").GridUnload();
        jQuery("#shipments").GridUnload();

        $.ajax({
            url: rootUrl + "Orders/DueDatesShipments?orderId=" + orderId,
            dataType: 'json',
            cache: false,
            success: function (data) {
                $order.showDueDates(data.duedates);
                $order.showShipments(data.shipments);
            }
        });

    },

    showDueDates: function (data) {
        var searchResultGrid = jQuery("#duedates").jqGrid({
            datatype: "jsonstring",
            datastr: data,
            colNames: ['Due Date', 'Quantity'],
            colModel: [
                { name: 'Due_dts', index: 'Due_dts', editable: false, sortable: true, sorttype: "date", formatter: "date", formatoptions: { srcformat: 'm/d/Y', newformat: 'm/d/Y'} },
                { name: 'Qty', index: 'Qty', editable: false, sortable: true }
   	        ],
            rowNum: 10,
            rowList: [10, 20, 30],
            height: 200,
            width: $(window).width() * .22,
            pager: '#duedatesPager',
            viewrecords: true,
            sortorder: 'asc',
            sortname: 'Due_dts',
            loadonce: true,
            sortable: true,
            ignoreCase: true,
            caption: "Due Dates",
            emptyDataText: "No due dates found"
        });
        jQuery("#duedates").jqGrid('navGrid', '#duedatesPager', { edit: false, add: false, del: false });
        $("#search_duedates").remove();
        $(".ui-jqgrid-titlebar").hide();
        $global.changeUpperRadius('gbox_duedates');
    },

    showShipments: function (data) {
        var searchResultGrid = jQuery("#shipments").jqGrid({
            datatype: "jsonstring",
            datastr: data,
            colNames: ['Packing List', 'Ship Date', 'Quantity', 'Ship Via', 'Tracking Number', 'Invoice Number', 'Invoice Date'],
            colModel: [
                { name: 'PackListNo', index: 'PackListNo', editable: false, sortable: true },
                { name: 'ShipDate', index: 'ShipDate', editable: false, sortable: true, sorttype: "date", formatter: "date", formatoptions: { srcformat: 'm/d/Y', newformat: 'm/d/Y'} },
                { name: 'ShippedQty', index: 'ShippedQty', editable: false, sortable: true },
                { name: 'ShipVia', index: 'ShipVia', editable: false, sortable: true },
                { name: 'WayBill', index: 'WayBill', editable: false, sortable: true },
                { name: 'InvoiceNo', index: 'InvoiceNo', editable: false, sortable: true },
                { name: 'InvDate', index: 'InvDate', editable: false, sortable: true, sorttype: "date", formatter: "date", formatoptions: { srcformat: 'm/d/Y', newformat: 'm/d/Y'} }
   	        ],
            rowNum: 10,
            rowList: [10, 20, 30],
            height: 200,
            width: $(window).width() * .74,
            pager: '#shipmentsPager',
            viewrecords: true,
            sortorder: 'asc',
            sortname: 'Due_dts',
            loadonce: true,
            sortable: true,
            ignoreCase: true,
            caption: "Due Dates",
            emptyDataText: "No due dates found"
        });
        jQuery("#shipments").jqGrid('navGrid', '#shipmentsPager', { edit: false, add: false, del: false });
        $("#search_shipments").remove();
        $(".ui-jqgrid-titlebar").hide();
        $global.changeUpperRadius('gbox_shipments');
    }
};
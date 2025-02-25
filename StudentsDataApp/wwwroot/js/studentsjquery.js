$(document).ready(function () {



    $("#addStudentBtn").click(function () {
        $("#studentForm")[0].reset();
        $("#studentId").val("");
        $("#studentModalLabel").text("Add Student");
        $(".error-message").remove();
    });

    $("#studentForm").submit(function (event) {
        event.preventDefault();


        var isValid = true;
        var firstName = $("#firstName").val().trim();
        var lastName = $("#lastName").val().trim();
        var rollNumber = $("#rollNumber").val().trim();
        var marks = $("#marks").val().trim();
        var email = $("#email").val().trim();

        $(".error-message").remove();

        if (firstName === "") {
            $("#firstName").after("<span class='error-message text-danger'>First name is required.</span>");
            isValid = false;
        }
        if (lastName === "") {
            $("#lastName").after("<span class='error-message text-danger'>Last name is required.</span>");
            isValid = false;
        }
        if (rollNumber === "" || isNaN(rollNumber)) {
            $("#rollNumber").after("<span class='error-message text-danger'>Valid roll number is required.</span>");
            isValid = false;
        }
        if (marks === "" || isNaN(marks) || marks < 0 || marks > 100) {
            $("#marks").after("<span class='error-message text-danger'>Marks must be between 0 and 100.</span>");
            isValid = false;
        }
        if (email === "" || !/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(email)) {
            $("#email").after("<span class='error-message text-danger'>Valid email is required.</span>");
            isValid = false;
        }

        if (!isValid) return;

        var formData = new FormData(this);
        var studentId = $("#studentId").val();
        var url = studentId ? "/Students/UpdateStudent" : "/Students/AddStudent";

        $.ajax({
            url: url,
            type: "POST",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.success) {
                    alert(studentId ? "Student updated successfully!" : "Student added successfully!");
                    $("#studentModal").modal("hide");
                    loadStudents();
                } else {
                    alert("Error: " + response.message);
                }
            },
            error: function () {
                alert("Something went wrong.");
            }
        });
    });




    function loadStudents() {
        $.ajax({
            url: "/Students/GetAllStudents",
            type: "GET",
            success: function (data) {
                var studentTableBody = $("#studentTableBody");
                studentTableBody.empty();

                $.each(data, function (index, student) {
                    var imageSrc = student.image ? `/Students/GetStudentImage?id=${student.id}` : "default.jpg";
                    var row = `<tr>
                        <td>${student.first_Name}</td>
                        <td>${student.last_Name}</td>
                        <td>${student.roll_Number}</td>
                        <td>${student.marks}</td>
                        <td>${student.email}</td>
                        <td><img src="${imageSrc}" width="50" height="50" /></td>
                        <td>
                            <button class="btn btn-primary editStudent" data-id="${student.id}">Edit</button>
                            <button class="btn btn-danger deleteStudent" data-id="${student.id}">Delete</button>
                        </td>
                    </tr>`;
                    studentTableBody.append(row);
                });

                $(".editStudent").click(function () {
                    $(".error-message").remove();
                    var studentId = $(this).data("id");
                    getStudentDetails(studentId);
                });

                $(".deleteStudent").click(function () {
                    var studentId = $(this).data("id");
                    deleteStudent(studentId);
                });
            }
        });
    }



    function getStudentDetails(studentId) {
        $.ajax({
            url: "/Students/GetStudentById?id=" + studentId,
            type: "GET",
            success: function (student) {
                if (student) {
                    $("#studentId").val(student.id);
                    $("#firstName").val(student.first_Name);
                    $("#lastName").val(student.last_Name);
                    $("#rollNumber").val(student.roll_Number);
                    $("#marks").val(student.marks);
                    $("#email").val(student.email);

                    $("#studentModalLabel").text("Edit Student");
                    $("#studentModal").modal("show");
                } else {
                    alert("Student not found!");
                }
            },
            error: function () {
                alert("Something went wrong while fetching student details.");
            }
        });
    }

    function deleteStudent(id) {
        if (confirm("Are you sure you want to delete this student?")) {
            $.ajax({
                url: "/Students/DeleteStudent?id=" + id,
                type: "DELETE",
                success: function (response) {
                    if (response.success) {
                        alert("Student deleted successfully!");
                        loadStudents();
                    } else {
                        alert("Error: " + response.message);
                    }
                }
            });
        }
    }

    loadStudents();
});

